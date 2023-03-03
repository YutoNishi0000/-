using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreateEnemy : MonoBehaviour
{
    public GameObject[] enemy;

    public GameObject[] enemyPoint;

    private List<int> point = new List<int>();

    private List<int> randNum = new List<int>();

    private float sec = 0;

    [SerializeField] private float _interval = 30;

    [SerializeField] private Text _startText;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(nameof(ShowText));

        for (int i = 0; i < enemyPoint.Length; i++)
        {
            randNum.Add(i);
        }

        for(int i = 0; i < 3; i++)
        {
            int index = Random.Range(0, randNum.Count);

            point.Add(randNum[index]);

            randNum.RemoveAt(index);
        }

        for (int j = 0; j < 3; j++)
        {
            Instantiate(enemy[Random.Range(0, enemy.Length)], enemyPoint[point[j]].transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        ControlleEnemy();
    }

    void ControlleEnemy()
    {
        sec += Time.deltaTime;

        if(sec > _interval)
        {
            //三体モンスターを生成
            for (int j = 0; j < 3; j++)
            {
                Instantiate(enemy[Random.Range(0, enemy.Length)], enemyPoint[point[j]].transform.position, Quaternion.identity);
            }

            sec = 0;
            //リストの中身をクリア
            point.Clear();

            //エネミーのIDをリストに追加
            for (int i = 0; i < enemyPoint.Length; i++)
            {
                randNum.Add(i);
            }

            //ランダムにリスト内の中身をシャッフルする
            for (int i = 0; i < 3; i++)
            {
                int index = Random.Range(0, randNum.Count);

                point.Add(randNum[index]);

                randNum.RemoveAt(index);
            }
        }
    }

    //コルーチン関数を定義
    private IEnumerator ShowText() //コルーチン関数の名前
    {
        yield return new WaitForSeconds(3.0f);
        _startText.enabled = false;
    }
}