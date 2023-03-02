using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//�V���O���g���̍쐬
public class PlayerState : MonoBehaviour
{
    //�V���O���g����
    public static PlayerState playerState;

    private void Awake()
    {
        if (playerState == null)
        {
            playerState = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //���g�̓���̏�Ԃ�\��
    public enum State
    {
        Attack,
        GetDamage,
        Dash,
        DropFriend,
        None
    }

    //�U�����̖��@�̑���
    public enum FireBall
    {
        red, 
        green,
        blue
    }

    //�V�[���X�e�[�g
    public enum ModeSelection
    {
        Title,
        BrightDay,
        MoodyNight,
    }

    //�Q�[���X�e�[�g
    public enum GemeState
    {
        None,
        GameOver,
        GameClear
    }

    //�����@�𐶐����邩
    public enum BurstState
    {
        Single, 
        Multi
    }

    public readonly int _offensivePower = 50000;//1000;        //�U����
    public readonly float PlayerMAXHP = 100000;
    
    public int NumCrushingEnemies;
    public float PlayerHP = 100000;
    public int NumFriends = 0;

    public State state;
    public FireBall fireBall;
    public ModeSelection modeSelection;
    public GemeState _gameState;
    public BurstState _burstState;

    private void Start()
    {
        modeSelection = ModeSelection.Title;
        PlayerHP = PlayerMAXHP;
        NumFriends = 5;
    }
}
