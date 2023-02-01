using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//�V���O���g���̍쐬
public class PlayerState : MonoBehaviour
{
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

    public State state;

    //�U�����̖��@�̑���
    public enum FireBall
    {
        red, 
        green,
        blue
    }

    public FireBall fireBall;

    public readonly int _offensivePower = 50000;//1000;        //�U����
    public int _defencePower = 100;          //�h���
    ////public uint _criticalRate = 50;          //��S��
    ////public uint _weaknessMultiplier = 100;    //��_�{��
    
    public int NumCrushingEnemies;

    //�����Ă��郊���S�̌�
    public int _NumApples = 0;

    public float PlayerHP = 100000;
    public readonly float PlayerMAXHP = 100000;
    public int NumFriends = 0;

    public enum ModeSelection
    {
        Title,
        BrightDay,
        MoodyNight,
    }

    public ModeSelection modeSelection;

    public enum GemeState
    {
        None,
        GameOver,
        GameClear
    }

    public GemeState _gameState;

    public enum BurstState
    {
        Single, 
        Multi
    }

    public BurstState _burstState;

    private void Start()
    {
        modeSelection = ModeSelection.Title;
        PlayerHP = PlayerMAXHP;
        NumFriends = 5;
    }
}
