using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

//プレイヤーが放つオブジェクトをオブジェクトプールによって制御する
public class PlayerObjectPool : MonoBehaviour
{
    //発射するオブジェクト(1番目に赤、2番目に緑、3番目に青を入れる)
    public GameObject fireObj;
    //発射するオブジェクトをあらかじめ格納するリスト型の多次元配列
    private List<GameObject> poolObjects = new List<GameObject>();
    //どのゲームオブジェクト直下に生成するか
    public GameObject ObjectPool;

    // Start is called before the first frame update
    void Start()
    {
        //for (int j = 0; j < fireObj.Length; j++)
        //{
            for (int i = 0; i < 20; i++)
            {
                GameObject obj = Instantiate(fireObj, ObjectPool.transform);
                obj.SetActive(false);
                poolObjects.Add(obj);
            }
        //}
    }

    /// <summary>
    /// 発射するオブジェクトを取得
    /// </summary>
    /// <param name="fireType">
    /// 魔法属性
    /// </param>
    /// <returns></returns>
    public GameObject GetPlayerFireObjects(PlayerState.FireBall fireType)
    {
        //Debug.Log(fireObj[(int)fireType]);
        //オブジェクトプールの中から非アクティブのオブジェクトを返す
        for (int i = 0; i < poolObjects.Count; i++)
        {
            //FireBall型の変数をint型にキャスト
            if (poolObjects[i].activeInHierarchy == false/* && poolObjects[i] == fireObj[(int)fireType]*/)
            {
                return poolObjects[i];
            }
        }

        return null;
    }
}
