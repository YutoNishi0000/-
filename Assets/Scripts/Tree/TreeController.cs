using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static EnemyController;
using UnityEngine.AI;

public class TreeController : Actor, ITreeController
{
    public Slider _bulkHPBar;
    public Slider _HPBar;

    [SerializeField] private int _treeHP;

    private Shaking _shaking;
    private float sec;
    private bool _getDamage = false;

    // Start is called before the first frame update
    void Start()
    {
        _treeHP = 1000000;
        _shaking = GetComponent<Shaking>();
    }

    // Update is called once per frame
    void Update()
    {
        if(_treeHP <= 0)
        {
            PlayerState.playerState._gameState = PlayerState.GemeState.GameOver;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //もしも木の近くに着たら
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();

            //移動スピードを0に
            enemy.navMeshAgent.speed = 0.0f;

            //攻撃処理
            enemy._enemyState = EnemyState.Attack;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Attack2"))
        {
            Debug.Log("木がダメージを受けている！！");

            _getDamage = true;
        }
    }

    public void Damage(int damage)
    {
        _treeHP -= damage;

        _bulkHPBar.value = _treeHP;

        _HPBar.DOValue(_treeHP, 0.3f);
    }
}

//木のダメージ処理用インターフェイス
public interface ITreeController
{
    public void Damage(int damageVal);
}
