using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvent : MonoBehaviour
{
    public bool _attack = false;
    public bool _endAttack = false;

    //�U���A�j���[�V�������Đ�����鎞�ɌĂяo�����
    void Attack()
    {
        //�U���J�n
        _attack = true;
        _endAttack = false;
    }

    //�U���A�j���[�V�������I������Ƃ��ɌĂяo�����
    void AttackEnd()
    {
        //�U���I��
        _attack = false;
        _endAttack = true;
    }
}
