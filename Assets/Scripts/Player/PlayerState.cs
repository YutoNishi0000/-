using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//ƒVƒ“ƒOƒ‹ƒgƒ“‚Ìì¬
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
    //©g‚Ì“®ì‚Ìó‘Ô‚ğ•\‚·
    public enum State
    {
        Attack,
        GetDamage,
        Dash,
        DropFriend,
        None
    }

    public State state;

    //UŒ‚‚Ì–‚–@‚Ì‘®«
    public enum FireBall
    {
        red, 
        green,
        blue
    }

    public FireBall fireBall;

    public readonly int _offensivePower = 50000;//1000;        //UŒ‚—Í
    public int _defencePower = 100;          //–hŒä—Í
    ////public uint _criticalRate = 50;          //‰ïS—¦
    ////public uint _weaknessMultiplier = 100;    //ã“_”{—¦
    
    public int NumCrushingEnemies;

    //‚Á‚Ä‚¢‚éƒŠƒ“ƒS‚ÌŒÂ”
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
