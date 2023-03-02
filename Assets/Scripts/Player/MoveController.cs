using UnityEngine;
using UnityEngine.UI;

//オブジェクト基底クラス
public class Actor : MonoBehaviour
{
    //PlayerController型の変数宣言
    protected MoveController Instance;

    private void Awake()
    {
        //PlayerController型のインスタンスを取得
        Instance = FindObjectOfType<MoveController>();
    }

    public virtual void Move() { }  //移動用仮想関数

    public virtual void Attack() { }  //攻撃用仮想関数

    public virtual void GetDamage() { }  //被ダメ用仮想関数

    /// <summary>
    /// 向かせたい方向に滑らかに回転させるための関数(どのクラスでもよく使うためすぐに呼び出せるようにした)
    /// </summary>
    /// <param name="directon">
    /// 向きたい方向であるターゲットの位置
    /// </param>
    /// <param name="myPos">
    /// 自分の位置
    /// </param>
    /// <param name="sec">
    /// 回転補間スピード
    /// </param>
    public void CorrectRotation(Vector3 directon, Vector3 myPos, float sec)
    {
        var targetRotation = Quaternion.LookRotation(directon - myPos);
        targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, sec);
    }

    public virtual void AnimationManager() { }
}

//ダメージ処理インターフェイス(プレイヤー用)
public interface IDamageable
{
    //ダメージをくらったときの処理
    public void Damage(int damegeVal);

    //死亡処理
    public void Death();
}

//経験値インターフェイス
public interface IExp
{
    public void LevelUp();

    public void GetExp(int exp);
}

public class MoveController : Actor
{
    private Vector3 NormalVector;  //コリジョンに接触している地面の法線ベクトルを格納
    //Animatorを入れる
    public Animator animator;
    //Main Cameraを入れる
    public Transform cam;
    public GameObject FirePos;   //魔法オブジェクトを生み出すための位置
    [System.NonSerialized] public Detection dec;             //Detectionクラスの変数を取得
    private Vector3 MoveVector;                     //移動方向を格納するベクトル
    private float _revivalTime;                      //復活までの時間を格納する
    public PlayerAnimationEvent animEve;
    private GetEnemyAttack _getDamage;
    public PlayerParameter param;
    public Slider _bulkHPBar;
    public Slider _HPBar;
    private Rigidbody rb;
    private float totalFallTime = 0f;
    private SePlay _sePlay;
    private bool _damageSE = false;
    private bool _isGround = false;
    public bool _isDropping;       //味方を召喚中かを表すフラグ

    // Start is called before the first frame update
    void Start()
    {
        //Animatorコンポーネントを取得
        animator = GetComponent<Animator>();

        //Detectionクラスインスタンスを取得
        dec = GetComponentInChildren<Detection>();

        //プレイヤーの動作状態を初期化
        PlayerState.playerState.state = PlayerState.State.None;

        //プレイヤーの攻撃魔法属性を赤属性にする
        PlayerState.playerState.fireBall = PlayerState.FireBall.red;

        //アニメーションイベント管理クラスのインスタンスを取得
        animEve = GetComponentInChildren<PlayerAnimationEvent>();

        //ダメージをくらったかどうかを管理するクラスのインスタンスを取得
        _getDamage = GetComponentInChildren<GetEnemyAttack>();

        rb = GetComponent<Rigidbody>();

        _bulkHPBar.value = PlayerState.playerState.PlayerMAXHP;

        _HPBar.value = PlayerState.playerState.PlayerMAXHP;

        _sePlay = GameObject.Find("AudioManager").GetComponent<SePlay>();

        //ゲーム開始ごとに敵の撃破数を初期化する
        PlayerState.playerState.NumCrushingEnemies = 0;

        //ゲーム開始時はプレイヤーのHPを初期化
        PlayerState.playerState.PlayerHP = PlayerState.playerState.PlayerMAXHP;

        _isDropping = false;
    }

    // Update is called once per frame
    void Update()
    {
        Death();
        Move();

        Debug.Log("HP" + PlayerState.playerState.PlayerHP);
    }

    //地面との接触判定を物理演算のフレームワークで行っているため合わせたい
    private void FixedUpdate()
    {
    }

    public override void Move()
    {
        //進行方向計算
        //キーボード入力を取得
        float v = Input.GetAxisRaw("Vertical");         //InputManagerの↑↓の入力       
        float h = Input.GetAxisRaw("Horizontal");       //InputManagerの←→の入力 

        //カメラの正面方向ベクトルからY成分を除き、正規化してキャラが走る方向を取得
        Vector3 forward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 right = Camera.main.transform.right; //カメラの右方向を取得

        switch (PlayerState.playerState.state)
        {
            case PlayerState.State.None:
                //キー入力があった場合だけ動かす
                MoveVector = h * right + v * forward;
                break;

            case PlayerState.State.Attack:
                //攻撃時は移動させなくさせる
                MoveVector = Vector3.zero;
                break;
        }

        //カメラの正面向きに対してキー入力ごとに回転をかける
        RotationControl(MoveVector);

        //斜面に沿ったベクトルを取得
        Vector3 OnPlane = Vector3.ProjectOnPlane(MoveVector, NormalVector);

        //落下処理
        if (!_isGround)
        {
            totalFallTime += Time.deltaTime;
            OnPlane.y = Physics.gravity.y * totalFallTime;
        }
        else
        {
            totalFallTime = 0f;
        }

        //ゲームクリア、またはゲームオーバー時は移動できないようにする
        if (PlayerState.playerState._gameState == PlayerState.GemeState.None)
        {
            rb.velocity = OnPlane * param._moveSpeed;         //歩く速度
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

        AnimationManager(rb);
    }

    // プレイヤーの回転処理
    void RotationControl(Vector3 moveVec)  //キャラクターが移動方向を変えるときの処理
    {
        Vector3 rotateDirection = moveVec;
        rotateDirection.y = 0;

        //それなりに移動方向が変化する場合のみ移動方向を変える
        if (rotateDirection.sqrMagnitude > 0.01)
        {
            //緩やかに移動方向を変える
            float step =  300 * Time.deltaTime;
            Vector3 newDir = Vector3.Slerp(transform.forward, rotateDirection, step);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // 衝突した面の、接触した点における法線を取得
            NormalVector = collision.GetContact(0).normal;
            _isGround = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGround = false;
        }
    }

    //主にアニメーションを管理する関数（歩行時以外）
    public void AnimationManager(Rigidbody rb)
    {
        //主にアニメーション管理を行う関数
        switch (PlayerState.playerState.state)
        {
            　　//攻撃するとき
            case PlayerState.State.Attack:
                animator.SetBool("Attack", true);
                break;

            　　//何もない時は歩きかアイドルモージョンを再生する
            case PlayerState.State.None:
                animator.SetBool("Attack", false);
                animator.SetBool("Damage", false);

                if (rb.velocity.magnitude > 0)
                {
                    //アニメーションを出す
                    animator.SetFloat("Walk", rb.velocity.magnitude);
                }
                else
                {
                    //何も押されていなかったら移動モーションを止めて0を返す
                    animator.SetFloat("Walk", rb.velocity.magnitude);
                }
                break;
        }
    }

    public void Death()
    {
        //死亡処理を実装するのを忘れない
        if(PlayerState.playerState.PlayerHP <= 0)
        {
            PlayerState.playerState._gameState = PlayerState.GemeState.GameOver;
        }
    }
}