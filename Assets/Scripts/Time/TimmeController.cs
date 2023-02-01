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
        //time.text = "Žc‚è" + Mathf.RoundToInt(_timeLeft / 60) + "•ª" + "00•b";
    }

    // Update is called once per frame
    void Update()
    {
        _timeLeft -= Time.deltaTime;

        int devide = Mathf.FloorToInt(_timeLeft / 60);

        int second = Mathf.FloorToInt(_timeLeft % 60);

        time.text = "Žc‚è" + devide + "•ª" + second + "•b";

        if(_timeLeft < 0 && PlayerState.playerState._gameState != PlayerState.GemeState.GameOver)
        {
            PlayerState.playerState._gameState = PlayerState.GemeState.GameClear;
            time.text = "I—¹I";
        }
    }
}