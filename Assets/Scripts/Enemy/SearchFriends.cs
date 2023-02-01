using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//プレイヤーの味方が自陣に入ってきたときの挙動を制御する
public class SearchFriends : Actor
{
    private EnemyController enemy;

    private void Start()
    {
        enemy = GetComponentInParent<EnemyController>();
    }
}