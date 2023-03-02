using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//シングルトンの作成
public class PlayerState : MonoBehaviour
{
    //シングルトン化
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

    //攻撃時の魔法の属性
    public enum FireBall
    {
        red, 
        green,
        blue
    }

    //シーンステート
    public enum ModeSelection
    {
        Title,
        BrightDay,
        MoodyNight,
    }

    //ゲームステート
    public enum GemeState
    {
        None,
        GameOver,
        GameClear
    }

    //何個魔法を生成するか
    public enum BurstState
    {
        Single, 
        Multi
    }

    public readonly int _offensivePower = 50000;//1000;        //攻撃力
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
