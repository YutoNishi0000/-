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
        }
        //味方を攻撃したら
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
        else if (other.gameObject.CompareTag("Tree"))
        {
            //インターフェイスを呼び出す
            ITreeController treeController = other.gameObject.GetComponent<ITreeController>();

            treeController.Damage(param._damage);
        }
    }
}