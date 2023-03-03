using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using DG.Tweening;

public class EnemyController : Actor, IEnemyDamagable
{
    public enum EnemyState
    {
        Walk,        //散策
        Attack,            //攻撃
        GetDamage
    }

    public enum DefenceState
    {
        red,
        green,
        blue
    }

    private int NIGHT_HP = 100000;
    private int NIGHT_DAMAGE = 20000;
    private int BRIGHT_HP = 50000;
    private int BRIGHT_DAMAGE = 5200;
    private int EnemyHP = 10000;
    private bool _attack;
    private bool _getHit = false;                //攻撃をくらったかどうか
    private GameObject _treeObj;
    private Animator anim;
    public NavMeshAgent navMeshAgent;
    public EnemyParameter param;
    public DefenceState _defenceState;
    public Image HPType;
    public EnemyState _enemyState;
    public Slider _bulkHPBar;
    public Slider _HPBar;
    public bool _isfriend;         //プレイヤーが出す味方キャラが自身の範囲内に存在しているか

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        _treeObj = GameObject.Find("forestpack_tree_1_leaf_1");
        SetDestination();
        navMeshAgent.avoidancePriority = Random.Range(0, 100);
        _enemyState = new EnemyState();
        anim = GetComponent<Animator>();

        _defenceState = new DefenceState();
        int Type = Random.Range(0, 3);
        _defenceState = (DefenceState)Type;
        GetMonsterType(_defenceState);
        _attack = true;
        _isfriend = false;

        //ナイトモードを選ぶと敵のステータスをアップさせる
        if(PlayerState.playerState.modeSelection == PlayerState.ModeSelection.MoodyNight)
        {
            EnemyHP = NIGHT_HP;
            _HPBar.value = NIGHT_HP;
            _bulkHPBar.maxValue = NIGHT_HP;
            _HPBar.maxValue = NIGHT_HP;
            _bulkHPBar.value = NIGHT_HP;
            param._damage = NIGHT_DAMAGE;
        }
        else if(PlayerState.playerState.modeSelection == PlayerState.ModeSelection.BrightDay)
        {
            EnemyHP = BRIGHT_HP;
            _HPBar.value = BRIGHT_HP;
            _bulkHPBar.maxValue = BRIGHT_HP;
            _HPBar.maxValue = BRIGHT_HP;
            _bulkHPBar.value = BRIGHT_HP;
            param._damage = BRIGHT_DAMAGE;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Death();
        AnimationManager();
        GetDamage();
        GameFinish();
        SetDestination();
    }

    private void LateUpdate()
    {
        GetRayhHit();
    }

    void SetDestination()
    {
        if (!_isfriend)
        {
            navMeshAgent.destination = _treeObj.transform.position;
        }
    }

    //アニメーションを管理する関数
    public override void AnimationManager()
    {
        switch(_enemyState)
        {
            case EnemyState.Walk:
                anim.SetBool("Walk", true);
                break;

            case EnemyState.Attack:
                anim.SetBool("Walk", false);

                Attack();
                break;
        }
    }

    void GetRayhHit()
    {
        GetComponent<Outline>().OutlineColor = Color.clear;
    }

    public override void Attack()
    {
        if(_attack)
        {
            //攻撃開始
            StartCoroutine(CAttack());
        }
    }

    IEnumerator CAttack()
    {
        anim.SetTrigger("Attack2");
        _attack = false;
        yield return new WaitForSeconds(3.0f);
        _attack = true;
    }

    void GetMonsterType(DefenceState state)
    {
        switch (state)
        {
            case DefenceState.red:
                HPType.color = Color.red;
                break;

            case DefenceState.green:
                HPType.color = Color.green;
                break;

            case DefenceState.blue:
                HPType.color = Color.blue;
                break;
        }
    }

    public void Damage(int damegeVal)
    {
        //ダメージ処理
        EnemyHP -= damegeVal;
        //HPを一気に減らす
        _bulkHPBar.value -= damegeVal;
        //DOTweenを使って滑らかにHPを減らしていく
        _HPBar.DOValue(_bulkHPBar.value, 1f);
    }

    //エネミーが死んだときの処理
    public void Death()
    {
        //死亡処理
        if(EnemyHP <= 0)
        {
            //PlayerState.playerState.NumCrushingEnemies++;
            anim.SetBool("Die", true);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Magic"))
        {
            _getHit = true;
        }
    }

    public override void GetDamage()
    {
        if(_getHit && EnemyHP > 0)
        {
            anim.SetTrigger("Damage");
            _getHit = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        //特定のオブジェクトに対して自身の向きを変えたい
        if (other.gameObject.CompareTag("Tree") || other.gameObject.CompareTag("Friend"))
        {
            //木の方向をを常に向き続けるようにする
            CorrectRotation(other.gameObject.transform.position, transform.position, 0.3f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Friend"))
        {
            _enemyState = EnemyState.Walk;
            navMeshAgent.speed = param._moveSpeed;
            _isfriend = false;
        }
    }

    //プレイヤーがゲームクリア、またはゲームオーバーになったら移動と攻撃をやめさせる
    void GameFinish()
    {
        if(PlayerState.playerState._gameState == PlayerState.GemeState.GameClear || PlayerState.playerState._gameState == PlayerState.GemeState.GameOver)
        {
            navMeshAgent.speed = 0f;
            _enemyState = EnemyState.Walk;
        }
    }
}

//敵用ダメージ処理インターフェイス
interface IEnemyDamagable
{
    void Damage(int damegeVal);

    void Death();
}