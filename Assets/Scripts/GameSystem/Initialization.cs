using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�Q�[���V�X�e���̏��������s���N���X
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
            //�v���C���[��HP�𖞃^���ɂ���
            //�v���C���[�̗̑͂�������
            PlayerState.playerState.PlayerHP = 0;
            //�̗͂𖞃^���ɂ���
            PlayerState.playerState.PlayerHP = PlayerState.playerState.PlayerMAXHP;
        }

        PlayerState.playerState._gameState = PlayerState.GemeState.None;
    }
}
