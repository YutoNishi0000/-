using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TreeController;

//攻撃時のエネミーの当たり判定処理
public class Damage : Actor
{
    private EnemyParameter param;

    private void Start()
    {
        param = GetComponentInParent<EnemyController>().param;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //インターフェイスを呼び出す
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.Damage(param._damage);
            }

            //damageable?.Damage(param._damage);
        }
        else if (other.gameObject.CompareTag("Friend"))
        {
            //インターフェイスを呼び出す
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                //ダメージ処理を実際に行う
                damageable.Damage(param._damage);
            }
        }
        //木を攻撃したら
        //if (PlayerState.playerState.modeSelection != PlayerState.ModeSelection.Adventure)
        {
            if (other.gameObject.CompareTag("Tree"))
            {
                //インターフェイスを呼び出す
                ITreeController treeController = other.gameObject.GetComponent<ITreeController>();

                treeController.Damage(param._damage);
            }
        }
    }

    //int CalcDamage(int damageVal, PlayerState.DefenceState defenceState, EnemyController.DefenceState enemyState)
    //{
    //    //プレイヤーの攻撃属性が自身（エネミー）の防御属性と同じだったら
    //    if ((int)defenceState == (int)enemyState)
    //    {
    //        //等倍
    //        int damage = FinalDamage(damageVal, PlayerState.playerState._defencePower);

    //        return damage;
    //    }
    //    //プレイヤーの攻撃属性が自身（エネミー）の防御属性に対して強かったら
    //    else if(enemyState == EnemyController.DefenceState.red && defenceState == PlayerState.DefenceState.green
    //        || enemyState == EnemyController.DefenceState.green && defenceState == PlayerState.DefenceState.blue
    //        || enemyState == EnemyController.DefenceState.blue && defenceState == PlayerState.DefenceState.red)
    //    {
    //        //二倍
    //        int damage = FinalDamage(damageVal * 2, PlayerState.playerState._defencePower);

    //        return damage;
    //    }
    //    //プレイヤーの攻撃属性が自身（エネミー）の防御属性に対して弱かったら
    //    else
    //    {
    //        //二分の一倍
    //        int damage = FinalDamage(damageVal / 2, PlayerState.playerState._defencePower);

    //        return damage;
    //    }
    //}

    //防御力を含めた最終的なプレイヤーのダメージ値
    int FinalDamage(int damageVal, int defenceVal)
    {
        int finaldamageVal = damageVal - defenceVal;

        if(finaldamageVal <= 0)
        {
            return 0;
        }
        else
        {
            return finaldamageVal;
        }
    }
}