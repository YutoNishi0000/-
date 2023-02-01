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
        StartCoroutine("Corou1");

        //_startText.enabled = false;

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
            for (int j = 0; j < 3; j++)
            {
                Instantiate(enemy[Random.Range(0, enemy.Length)], enemyPoint[point[j]].transform.position, Quaternion.identity);
            }

            sec = 0;
            point.Clear();

            for (int i = 0; i < enemyPoint.Length; i++)
            {
                randNum.Add(i);
            }

            for (int i = 0; i < 3; i++)
            {
                int index = Random.Range(0, randNum.Count);

                point.Add(randNum[index]);

                randNum.RemoveAt(index);
            }
        }
    }

    //コルーチン関数を定義
    private IEnumerator Corou1() //コルーチン関数の名前
    {
        //if (PlayerState.playerState.modeSelection != PlayerState.ModeSelection.Adventure)
        {
            //コルーチンの内容
            Debug.Log("スタート");
            yield return new WaitForSeconds(3.0f);
            _startText.enabled = false;
            Debug.Log("スタートから5秒後");
        }
    }
}