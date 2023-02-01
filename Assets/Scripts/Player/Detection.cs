using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//範囲内に入った敵の位置情報を取得する
public class Detection : MonoBehaviour
{
    private Vector3 EnemyPosition;    //エネミーの位置情報を格納する変数
    public bool EnemyDetection;       //エネミーが居るかどうか
    private GameObject[] detectObjects;   //フィールド内にいるエネミーのオブジェクトを格納するための変数

    private void Start()
    {
        EnemyDetection = false;
    }

    //この関数が呼び出された時だけエネミーの位置情報を取得する
    public Vector3 GetEnemyPos()
    {
        if (detectObjects != null)
        {
            //エネミーの位置情報を取得
            SearchEnemy();
        }
        return EnemyPosition;
    }

    void SearchEnemy()
    {
        //特定のタグのゲームオブジェクトを格納
        detectObjects = GameObject.FindGameObjectsWithTag("Enemy");

        //初期値の設定
        float closeDist = 1000;

        foreach (GameObject t in detectObjects)
        {
            //取得したオブジェクトとの距離を計測する
            float tDist = Vector3.Distance(transform.position, t.transform.position);

            //もし初期値よりも計測した敵までの距離が近ければ
            if (closeDist > tDist)
            {
                //自身と敵との距離を上書きする
                closeDist = tDist;

                //エネミーの場所を格納
                EnemyPosition = new Vector3(t.transform.position.x, t.transform.position.y + 0.5f, t.transform.position.z);

                Debug.Log("一番近い敵のオブジェクトを取得");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //範囲内にエネミーが居たら
            EnemyDetection = true;
            Debug.Log("敵が進入してきた");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            //範囲からエネミーがいなくなったら
            EnemyDetection = false;
        }
    }
}
