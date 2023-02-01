using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ゲームシステムの初期化を行うクラス
public class Initialization : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerState.playerState._gameState == PlayerState.GemeState.GameOver)
        {
            PlayerState.playerState.modeSelection = PlayerState.ModeSelection.Title;
            //プレイヤーのHPを満タンにする
            //プレイヤーの体力を初期化
            PlayerState.playerState.PlayerHP = 0;
            //体力を満タンにする
            PlayerState.playerState.PlayerHP = PlayerState.playerState.PlayerMAXHP;
        }

        PlayerState.playerState._gameState = PlayerState.GemeState.None;
    }
}
