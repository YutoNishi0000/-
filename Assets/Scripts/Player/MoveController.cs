using DG.Tweening;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

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
    private bool _isGround = false;
    private SePlay _sePlay;
    private bool _damageSE = false;
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
        AnimationManager();
        //Heal();

        Debug.Log("HP" + PlayerState.playerState.PlayerHP);
    }

    //地面との接触判定を物理演算のフレームワークで行っているため合わせたい
    private void FixedUpdate()
    {
        Move();
    }

    public override void Move()
    {
        //x, z 平面での移動
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        //Vector3 target_dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //前移動の時で攻撃時以外の時はカメラの前方向を向かせる
        if (z > 0 && PlayerState.playerState.state != PlayerState.State.Attack)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(transform.rotation.x, cam.eulerAngles.y, transform.rotation.z)), 3.0f);
        }

        ////カメラの正面向きに対してキー入力ごとに回転をかける
        Rotation(x, z);
        ////transform.position += transform.forward * z + transform.right * x;

        switch (PlayerState.playerState.state)
        {
            case PlayerState.State.None:
                //キー入力があった場合だけ動かす
                MoveVector = transform.forward * GetKeyInput(x, z);/* + transform.right * x*/;
                break;

            case PlayerState.State.Attack:
                //攻撃時は移動させなくさせる
                MoveVector = Vector3.zero;
                break;

            case PlayerState.State.GetDamage:
                //移動できないようにする
                break;
        }

        //急な坂道でも登れるようにするため地面から得た法線ベクトルと入力方向に対するベクトルを使って斜面に平行なベクトルを得る
        //地面から得る法線ベクトルはコリジョンを使うためこの関数はFixedUpdateで実行する

        //斜面に沿ったベクトルを取得
        Vector3 OnPlane = Vector3.ProjectOnPlane(MoveVector, NormalVector);
        //Vector3 OnPlane = MoveVector - NormalVector;

        if (!_isGround)
        {
            totalFallTime += Time.deltaTime;
            OnPlane.y = Physics.gravity.y * totalFallTime;
            Debug.Log("地面についていないよー－－－－－－－－");
        }
        else
        {
            totalFallTime = 0f;
            Debug.Log("じめんについているよー－－－－－－");
        }

        //ゲームクリア、またはゲームオーバー時は移動できないようにする
        if (PlayerState.playerState._gameState != PlayerState.GemeState.GameClear && PlayerState.playerState._gameState != PlayerState.GemeState.GameOver)
        {
            rb.velocity = OnPlane * Avoidance(param._moveSpeed, param._moveSpeed * 3f);         //歩く速度
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

        ////斜面に沿ったベクトルを使って移動させる
        //transform.position += /*param._moveSpeed*/Avoidance(param._moveSpeed, param._moveSpeed * 1.5f) * Time.deltaTime * OnPlane;/*transform.forward * z + transform.right * x*/;
    }

    //進行方向に対してプレイヤーオブジェクトの回転をかける関数
    void Rotation(float AxisX, float AxisZ)
    {
        //ここではプレイヤーの回転処理を主に行う関数

        //ただの移動の時だけこの関数を実行する（攻撃中は実行しない）

        if (PlayerState.playerState.state == PlayerState.State.None)
        {
            //右入力
            if (AxisX > 0)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, cam.eulerAngles.y + 90, transform.rotation.z), 5.0f);
            }
            //左入力
            if (AxisX < 0)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, cam.eulerAngles.y - 90, transform.rotation.z), 5.0f);
            }
            //上入力
            if (AxisZ > 0)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, cam.eulerAngles.y + 0, transform.rotation.z), 5.0f);
            }
            //下入力
            if (AxisZ < 0)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, cam.eulerAngles.y + 180, transform.rotation.z), 7.0f);
            }
        }
    }

    //キーの入力がなされたらfloat型の値(1)を返す関数
    float GetKeyInput(float AxisX, float AxisZ)
    {
        if(AxisX != 0 || AxisZ != 0)
        {
            //アニメーションを出す
            animator.SetBool("Walk", true);
            return 1.0f;
        }
        else
        {
            //何も押されていなかったら移動モーションを止めて0を返す
            animator.SetBool("Walk", false);
            return 0.0f;
        }
    }

    //回避を行う関数
    float Avoidance(float speed, float acceleration)
    {
        //if(Input.GetMouseButton(1))
        //{
        //    return acceleration;
        //}
        //else if(Input.GetMouseButtonUp(1))
        //{
        //    float movespeed = Mathf.MoveTowards(acceleration, speed, Time.deltaTime);

        //    return movespeed;
        //}
        //else
        //{
        //    return speed;
        //}
        return speed;
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
    public override void AnimationManager()
    {
        //主にアニメーション管理を行う関数
        switch (PlayerState.playerState.state)
        {
            　　//攻撃するとき
            case PlayerState.State.Attack:
                animator.SetBool("Attack", true);
                break;

            //　　//ダメージを受けたとき
            //case PlayerState.State.GetDamage:
            //    animator.SetBool("Damage", true);
            //    Debug.Log("敵からダメージをくらった");
            //    break;

            　　//何もない時は歩きかアイドルモージョンを再生する
            case PlayerState.State.None:
                animator.SetBool("Attack", false);
                animator.SetBool("Damage", false);
                break;
        }
    }

    //public override void GetDamage()
    //{
    //    //if (_getDamage.IsGetHit)
    //    //{
    //    //    if (!_damageSE)
    //    //    {
    //    //        _sePlay.Play("GET_DAMAGE");
    //    //        _damageSE = true;
    //    //    }

    //    //    //復帰するまでの時間を計測
    //    //    _revivalTime += Time.deltaTime;

    //    //    //プレイヤーステートをGetDamageに
    //    //    PlayerState.playerState.state = PlayerState.State.GetDamage;

    //    //    if(_revivalTime > param._revivalTime)
    //    //    {
    //    //        //もしも復帰できるようになったらステートをNoneに戻す
    //    //        _revivalTime = 0;
    //    //        PlayerState.playerState.state = PlayerState.State.None;
    //    //        _getDamage.IsGetHit = false;
    //    //        _damageSE = false;
    //    //    }
    //    //}
    //}

    //ダメージ処理の詳細
    //public void Damage(int damegeVal)
    //{
    //    //ダメージ処理
    //    PlayerState.playerState.PlayerHP -= damegeVal;

    //    //HPを一気に減らす
    //    _bulkHPBar.value -= damegeVal;

    //    //DOTweenを使って滑らかにHPを減らしていく
    //    _HPBar.DOValue(_bulkHPBar.value, 1f);
    //}

    public void Death()
    {
        //死亡処理を実装するのを忘れない
        if(PlayerState.playerState.PlayerHP <= 0)
        {
            PlayerState.playerState._gameState = PlayerState.GemeState.GameOver;
        }
    }
}

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