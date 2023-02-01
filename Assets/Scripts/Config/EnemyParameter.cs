using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy")]

public class EnemyParameter : ScriptableObject
{
    //�ړ��X�s�[�h
    public float _moveSpeed = 3.5f;

    //�G�̍��W����ǂꂾ�����ꂽ�Ƃ���܂ŎU�􂷂邩
    public float _wanderRange = 8.0f;

    //�ǔ��J�n���̃v���C���[�ƓG�̈ʒu
    public float _followDis = 10f;

    //�U���J�n���̃v���C���[�ƓG�̈ʒu
    public float _attackDis = 3.5f;

    //�U���̃N�[���^�C��
    public float _coolTime = 3.0f;

    //�_���[�W���󂯂Ă��畜�A����܂ł̎���
    public float _revivalTime = 0.3f;

    //�G�l�~�[���v���C���[�ɗ^����_���[�W��
    public int _damage = 300;

    public int _enemyHP = 10000;

    public int _level = 1;
}
