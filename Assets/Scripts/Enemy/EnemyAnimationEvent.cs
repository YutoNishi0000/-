using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvent : Actor
{
    public Collider[] col;
    public bool _endAttck2 = false;
    public bool _death = false;
    private EnemyParameter param;

    private void Start()
    {
        _endAttck2 = false;
        _death = false;
        param = GetComponent<EnemyController>().param;
        for (int i = 0; i < col.Length; i++)
        {
            col[i].enabled = false;
        }
    }

    //攻撃開始時に呼び出す関数
    void StartAttack2()
    {
        for(int i = 0; i < col.Length; i++)
        {
            col[i].enabled = true;
        }
        Debug.Log("アタック開始");
    }

    //攻撃が終わった後に呼び出す関数
    void EndAttack2()
    {
        for (int i = 0; i < col.Length; i++)
        {
            col[i].enabled = false;
        }
        Debug.Log("アタック終わり");
    }

    //アニメーションが始まった直後に呼び出す関数
    void StartAnimation()
    {
        _endAttck2 = false;
    }

    //アニメーションが終わった後に呼び出す関数
    void EndAnimation()
    {
        _endAttck2 = true;
    }

    void Death()
    {
        _death = true;
        Debug.Log("エネミーが死んだ！！");
        Destroy(this.gameObject);
        PlayerState.playerState.NumCrushingEnemies++;
        MissionManager._end = false;
        Debug.Log("敵撃破数" + PlayerState.playerState.NumCrushingEnemies);
    }
}