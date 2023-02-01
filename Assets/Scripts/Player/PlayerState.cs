using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//シングルトンの作成
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
    //自身の動作の状態を表す
    public enum State
    {
        Attack,
        GetDamage,
        Dash,
        DropFriend,
        None
    }

    public State state;

    //攻撃時の魔法の属性
    public enum FireBall
    {
        red, 
        green,
        blue
    }

    public FireBall fireBall;

    public readonly int _offensivePower = 50000;//1000;        //攻撃力
    public int _defencePower = 100;          //防御力
    ////public uint _criticalRate = 50;          //会心率
    ////public uint _weaknessMultiplier = 100;    //弱点倍率
    
    public int NumCrushingEnemies;

    //持っているリンゴの個数
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
