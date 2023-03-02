using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelection : MonoBehaviour
{
    //ボタンを押すとシーン遷移を行うためのクラス

    [SerializeField] private SePlay _sePlay;

    private void Start()
    {
        _sePlay = GameObject.Find("AudioManager").GetComponent<SePlay>();
    }

    //簡単なステージに移行する
    public void BrightDay()
    {
        SceneManager.LoadScene("BrightDay");
        PlayerState.playerState.modeSelection = PlayerState.ModeSelection.BrightDay;
        _sePlay.Play("Button");
    }

    //難しいステージに移行する
    public void MoodyNight()
    {
        SceneManager.LoadScene("MoodyNight");
        PlayerState.playerState.modeSelection = PlayerState.ModeSelection.MoodyNight;
        _sePlay.Play("Button");
    }

    //タイトルに戻る
    public void BackToTitle()
    {
        SceneManager.LoadScene("Title");
        PlayerState.playerState.modeSelection = PlayerState.ModeSelection.Title;
        _sePlay.Play("Button");
    }
}