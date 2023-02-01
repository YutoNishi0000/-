using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Enemy")]

public class EnemyParameter : ScriptableObject
{
    //移動スピード
    public float _moveSpeed = 3.5f;

    //敵の座標からどれだけ離れたところまで散策するか
    public float _wanderRange = 8.0f;

    //追尾開始時のプレイヤーと敵の位置
    public float _followDis = 10f;

    //攻撃開始時のプレイヤーと敵の位置
    public float _attackDis = 3.5f;

    //攻撃のクールタイム
    public float _coolTime = 3.0f;

    //ダメージを受けてから復帰するまでの時間
    public float _revivalTime = 0.3f;

    //エネミーがプレイヤーに与えるダメージ量
    public int _damage = 300;

    public int _enemyHP = 10000;

    public int _level = 1;
}
