using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelection : MonoBehaviour
{
    //�{�^���������ƃV�[���J�ڂ��s�����߂̃N���X

    [SerializeField] private SePlay _sePlay;

    private void Start()
    {
        _sePlay = GameObject.Find("AudioManager").GetComponent<SePlay>();
    }

    //�ȒP�ȃX�e�[�W�Ɉڍs����
    public void BrightDay()
    {
        SceneManager.LoadScene("BrightDay");
        PlayerState.playerState.modeSelection = PlayerState.ModeSelection.BrightDay;
        _sePlay.Play("Button");
    }

    //����X�e�[�W�Ɉڍs����
    public void MoodyNight()
    {
        SceneManager.LoadScene("MoodyNight");
        PlayerState.playerState.modeSelection = PlayerState.ModeSelection.MoodyNight;
        _sePlay.Play("Button");
    }

    //�^�C�g���ɖ߂�
    public void BackToTitle()
    {
        SceneManager.LoadScene("Title");
        PlayerState.playerState.modeSelection = PlayerState.ModeSelection.Title;
        _sePlay.Play("Button");
    }
}