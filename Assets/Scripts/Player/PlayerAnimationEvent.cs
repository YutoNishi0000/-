using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    public bool _attack = false;
    public bool _endAttack = false;

    //攻撃アニメーションが再生される時に呼び出される
    void Attack()
    {
        //攻撃開始
        _attack = true;
        _endAttack = false;
    }

    //攻撃アニメーションが終了するときに呼び出される
    void AttackEnd()
    {
        //攻撃終了
        _attack = false;
        _endAttack = true;
    }
}
