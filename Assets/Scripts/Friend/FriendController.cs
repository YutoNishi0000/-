using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static EnemyController;

//味方となるキャラクターを制御するクラス
public class FriendController : Actor, IDamageable
{
    private bool _isGround;   //地面についているか
    private bool _isDropped;
    public bool _isEnemy;    //敵が近くにいるかどうか
    public Slider _bulkHPBar;
    public Slider _HPBar;
    [SerializeField] private int _friendHP = 100000;
    private Animator anim;
    [SerializeField] private float _coolTime;
    public Collider swordCol;
    private bool _isDead;

    // Start is called before the first frame update
    void Start()
    {
        _isDead = false;
        _isGround = false;
        _isDropped = false;
        _isEnemy = false;
        anim = GetComponent<Animator>();
        _coolTime = 3.0f;
        swordCol.enabled = false;
        _friendHP = 100000;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isDropped)
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z + 10);
            transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }

        if(Input.GetMouseButtonDown(0))
        {
            _isDropped = true;
            Instance._isDropping = false;
            GetComponent<Rigidbody>().isKinematic = false;
        }

        Attack();
        Death();
        _isEnemy= false;
    }

    //攻撃処理
    public override void Attack()
    {
        //エネミーが近くに居たら
        if(_isEnemy)
        {
            //攻撃開始
            StartCoroutine(CAttack());
        }
        else
        {
            return;
        }
    }

    IEnumerator CAttack()
    {
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(_coolTime); 
    }

    //ダメージ処理の詳細
    public void Damage(int damegeVal)
    {
        //ダメージ処理
        _friendHP -= damegeVal;

        //HPを一気に減らす
        _bulkHPBar.value -= damegeVal;

        //DOTweenを使って滑らかにHPを減らしていく
        _HPBar.DOValue(_bulkHPBar.value, 1f);
    }

    public void Death()
    {
        //死亡処理を実装するのを忘れない
        if (_friendHP <= 0)
        {
            //プレイヤーのところに戻る
            transform.position = Vector3.MoveTowards(transform.position, Instance.transform.position, 0.1f);
            _isDead = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") && _isDead)
        {
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //もしもプレイヤーの味方だったら
        if (other.gameObject.CompareTag("Enemy"))
        {
            _isEnemy = true;    //エネミーが近くにいるというフラグをオンにする
            EnemyController enemy = other.GetComponent<EnemyController>();
            //フラグをオンにして
            enemy._isfriend = true;
            //速度を0にする
            enemy.navMeshAgent.speed = 0.0f;
            //目的地をそのキャラクターにする
            enemy.navMeshAgent.destination = transform.position;
            //攻撃処理
            enemy._enemyState = EnemyState.Attack;
            //敵の方向をを常に向き続けるようにする
            CorrectRotation(enemy.gameObject.transform.position, transform.position, 0.3f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            _isEnemy = false;
        }
    }

    //攻撃アニメーションが始める時に呼び出す
    public void StartAttack()
    {
        swordCol.enabled = true;
    }

    //攻撃アニメーションが終わるときに呼び出す
    public void EndAttack()
    {
        swordCol.enabled = false;
    }
}