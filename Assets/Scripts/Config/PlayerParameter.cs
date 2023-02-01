using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player")]

public class PlayerParameter : ScriptableObject
{
    //�ړ��X�s�[�h
    public float _moveSpeed = 5.0f;

    //�ʏ�U���̃N�[���^�C��
    public float _coolTime = 3.0f;

    //�_���[�W��������Ă��畜�A����܂ł̎���
    public float _revivalTime = 0.3f;
}
