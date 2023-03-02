using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//UIを管理するクラス
public class UIController : MonoBehaviour
{
    public Image red;
    public Image green;
    public Image blue;
    //public Text apples;
    public Text levels;
    [SerializeField] private Text gameOver;
    [SerializeField] private Text gameClear;
    [SerializeField] private Text NumFriends;
    [SerializeField] private Image blackPanel;
    [SerializeField] private float a_text;
    [SerializeField] private float sec;
    private SePlay _sePlay;
    private bool _seOneShot;

    // Start is called before the first frame update
    void Start()
    {
        red.enabled = false;
        green.enabled = false;
        blue.enabled = false;
        gameOver.enabled = false;
        gameClear.enabled = false;
        blackPanel.enabled = false;
        _seOneShot = false;
        a_text = 0;
        _sePlay = GameObject.Find("AudioManager").GetComponent<SePlay>();
    }

    // Update is called once per frame
    void Update()
    {
        //所持している味方の数を表示
        NumFriends.text = "x" + PlayerState.playerState.NumFriends;
        Type();

        switch (PlayerState.playerState._gameState)
        {
            case PlayerState.GemeState.GameOver:
                GameFinish(gameOver, "GAMEOVER");
                break;

            case PlayerState.GemeState.GameClear:
                GameFinish(gameClear, "GAMECLEAR");
                break;
        }
    }

    void Type()
    {
        //攻撃タイプに応じてUIの種類を変えていく
        switch (PlayerState.playerState.fireBall)
        {
            case PlayerState.FireBall.red:
                red.enabled = true;
                green.enabled = false;
                blue.enabled = false;
                break;

            case PlayerState.FireBall.green:
                green.enabled = true;
                red.enabled = false;
                blue.enabled = false;
                break;

            case PlayerState.FireBall.blue:
                blue.enabled = true;
                red.enabled = false;
                green.enabled = false;
                break;
        }
    }

    //ゲームオーバー、ゲームクリア時それぞれのステートに応じてゲーム終了処理を行う
    void GameFinish(Text gameState, string BGMName)
    {
        if (!_seOneShot)
        {
            _sePlay.Play(BGMName);
            _seOneShot = true;
        }
        Debug.Log("ゲームオーバーです");
        gameState.enabled = true;
        blackPanel.enabled = true;
        gameState.color = new Color(gameState.color.r, gameState.color.g, gameState.color.b, a_text);
        blackPanel.color = new Color(blackPanel.color.r, blackPanel.color.g, blackPanel.color.b, a_text);

        a_text += Time.deltaTime / 3;

        if (a_text >= 1)
        {
            a_text = 1;
            sec += Time.deltaTime;

            if (sec > 10)
            {
                sec = 0;
                PlayerState.playerState.modeSelection = PlayerState.ModeSelection.Title;
                SceneManager.LoadScene("Title");
            }
        }
    }
}