using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player")]

public class PlayerParameter : ScriptableObject
{
    //移動スピード
    public float _moveSpeed = 5.0f;

    //通常攻撃のクールタイム
    public float _coolTime = 3.0f;

    //ダメージをくらってから復帰するまでの時間
    public float _revivalTime = 0.3f;
}
