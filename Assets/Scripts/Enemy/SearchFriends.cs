using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�v���C���[�̖��������w�ɓ����Ă����Ƃ��̋����𐧌䂷��
public class SearchFriends : Actor
{
    private EnemyController enemy;

    private void Start()
    {
        enemy = GetComponentInParent<EnemyController>();
    }
}