using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    public bool _attack = false;
    public bool _endAttack = false;

    void Attack()
    {
        _attack = true;
        Debug.Log("プレイヤー攻撃開始");
        _endAttack = false;
    }

    void AttackEnd()
    {
        _attack = false;
        Debug.Log("プレイヤー攻撃終わり");
        _endAttack = true;
    }
}
