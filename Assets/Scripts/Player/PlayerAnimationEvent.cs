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
        Debug.Log("�v���C���[�U���J�n");
        _endAttack = false;
    }

    void AttackEnd()
    {
        _attack = false;
        Debug.Log("�v���C���[�U���I���");
        _endAttack = true;
    }
}
