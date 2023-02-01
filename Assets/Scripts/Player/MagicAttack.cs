using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttack : Actor
{
    public GameObject[] fireBalls;
    public float speed;
    private bool _attack = false;
    private Quaternion _cameraAngle;
    private Quaternion _cameraRotation;
    private float _coolTime;           //クールタイムの時間を格納
    [SerializeField] private SePlay _sePlay;
    public GameObject[] _multiFirePos;
    public bool _isRayHit;  //マウスから飛ばしたRayがエネミーにヒットしているかどうか
    [SerializeField]
    Transform target;
    [SerializeField]
    GameObject prefab;
    [SerializeField, Min(1)]
    int iterationCount = 3;
    [SerializeField]
    float interval = 0.1f;
    bool isSpawning = false;
    Transform thisTransform;
    WaitForSeconds intervalWait;
    private bool _isCoolTime;

    // Start is called before the first frame update
    void Start()
    {
        _isCoolTime = false;
        _isRayHit = false;
        _sePlay = GameObject.Find("AudioManager").GetComponent<SePlay>();
        thisTransform = transform;
        intervalWait = new WaitForSeconds(interval);
    }

    // Update is called once per frame
    void Update()
    {
        //if (isSpawning)
        //{
        //    return;
        //}
        //StartCoroutine(nameof(SpawnMissile));

        Move();
        SwitchFireType();
        CheckEnemy();
    }

    //マウスカーソル上にエネミーがいた場合の処理を記述
    void CheckEnemy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Rayを生成
        RaycastHit hit;
        //レイが衝突したオブジェクトのレイヤーがエネミー以外の場合はスルーさせる
        int layerMask = 1 << 13;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) // Rayを投射
        {
            //もしもタグがエネミーだったら
            if (hit.collider.CompareTag("Enemy")) // タグを比較
            {
                //モデルの輪郭を赤色で描画する
                hit.collider.gameObject.GetComponent<Outline>().OutlineColor = Color.red;

                //ここでクリックしたら攻撃処理を行いたい
                if (PlayerState.playerState._gameState != PlayerState.GemeState.GameClear && PlayerState.playerState._gameState != PlayerState.GemeState.GameOver)
                {
                    //ボタンを押したとき、クールタイムが終わってて味方召喚中でなければ
                    if (Input.GetMouseButtonDown(0) && _coolTime <= 0 && !Instance._isDropping)
                    {
                        PlayerState.playerState.state = PlayerState.State.Attack;
                        _attack = false;
                        _isCoolTime = true;
                        _coolTime = Instance.param._coolTime;
                        //Instance.transform.rotation = Quaternion.RotateTowards(Instance.transform.rotation, _cameraRotation, 10f);
                        //_cameraAngle = Quaternion.Euler(new Vector3(Instance.transform.rotation.x, Instance.cam.eulerAngles.y, Instance.transform.rotation.z));
                        //_cameraRotation = Quaternion.Euler(Instance.cam.eulerAngles);
                    }
                }
            }
            target = hit.collider.gameObject.transform;
        }
    }

    public override void Move()
    {
        if(PlayerState.playerState.state == PlayerState.State.Attack)
        {
            //攻撃モーションが始まっていたら、魔法攻撃を行う
            if (Instance.animEve._attack)
            {
                //一つだけ魔法玉を発射できれば良いので1フレーム分だけ攻撃処理を行う
                if (!_attack)
                {
                    _sePlay.Play("Attack");
                    Attack();
                    _attack = true;
                }
            }
            //クリックしてからアニメージョンが始まるまでの間
            else if(!Instance.animEve._attack && !_attack)
            {
                if(target == null)
                {
                    Debug.Log("標的が失われました");
                    return;
                }
                //選択した敵の方向を向くようにする
                Instance.transform.rotation = Quaternion.RotateTowards(Instance.transform.rotation, Quaternion.LookRotation(target.position - Instance.transform.position), 10f);

            }

            //攻撃モーションが終わったら、通常状態に戻る
            if (Instance.animEve._endAttack)
            {
                PlayerState.playerState.state = PlayerState.State.None;
                Instance.animEve._endAttack = false;
            }
        }

        if (_isCoolTime)
        {
            //クールタイムから経過時間を引くことで値が0以下になりクールタイムが終わる仕組み
            _coolTime -= Time.deltaTime;

            if (_coolTime < 0)
            {
                //スタックオーバーフローを発生させないためにクールタイムを格納する値が0を下回ったら0を代入する
                _coolTime = 0;
                _isCoolTime = false;
            }
        }
    }

    void SwitchFireType()
    {
        //1を押したら赤属性
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerState.playerState.fireBall = PlayerState.FireBall.red;
            _sePlay.Play("CHANGE_SE_ATTACK");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerState.playerState.fireBall = PlayerState.FireBall.green;
            _sePlay.Play("CHANGE_SE_ATTACK");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayerState.playerState.fireBall = PlayerState.FireBall.blue;
            _sePlay.Play("CHANGE_SE_ATTACK");
        }
    }

    public override void Attack()
    {
        if (PlayerState.playerState._burstState == PlayerState.BurstState.Single)
        {
            //魔法の種類を指定して攻撃を行う
            MagicObj homing;
            homing = Instantiate(fireBalls[(int)PlayerState.playerState.fireBall], transform.position, Quaternion.identity).GetComponent<MagicObj>();
            homing.Target = target;
        }
        //else if(PlayerState.playerState._burstState == PlayerState.BurstState.Multi)
        //{
        //    for(int i = 0; i < _multiFirePos.Length; i++)
        //    {
        //        //魔法の種類を指定して攻撃を行う
        //        MagicObj homing;
        //        homing = Instantiate(fireBalls[(int)PlayerState.playerState.fireBall], _multiFirePos[i].transform.position, Quaternion.identity).GetComponent<MagicObj>();
        //        homing.Target = target;
        //    }
        //}
    }
}