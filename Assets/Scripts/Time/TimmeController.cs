using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimmeController : MonoBehaviour
{
    [SerializeField] private Text time;
    private float _timeLeft;

    // Start is called before the first frame update
    void Start()
    {
        _timeLeft = 179.9999f;
        //time.text = "残り" + Mathf.RoundToInt(_timeLeft / 60) + "分" + "00秒";
    }

    // Update is called once per frame
    void Update()
    {
        _timeLeft -= Time.deltaTime;

        //何分か
        int devide = Mathf.FloorToInt(_timeLeft / 60);

        //何秒か
        int second = Mathf.FloorToInt(_timeLeft % 60);

        //残り時間を表示
        time.text = "残り" + devide + "分" + second + "秒";

        //時間が過ぎたらゲーム重量処理を行う
        if(_timeLeft < 0 && PlayerState.playerState._gameState != PlayerState.GemeState.GameOver)
        {
            PlayerState.playerState._gameState = PlayerState.GemeState.GameClear;
            time.text = "終了！";
        }
    }
}