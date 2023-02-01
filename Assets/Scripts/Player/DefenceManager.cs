using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DefenceManager : Actor
{
    private Image _defenceState;

    [SerializeField] private SePlay _sePlay;

    // Start is called before the first frame update
    void Start()
    {
        _defenceState = GetComponent<Image>();
        _sePlay = GameObject.Find("AudioManager").GetComponent<SePlay>();
    }

    //// Update is called once per frame
    //void Update()
    //{
    //    ChangeDefenceState();
    //}

    //void ChangeDefenceState()
    //{
    //    if(Input.GetKeyDown(KeyCode.Alpha4))
    //    {
    //        PlayerState.playerState.defenceState = PlayerState.DefenceState.red;
    //        _sePlay.Play("CHANGE_SE_DEFENCE");
    //        //UI‚Ì‘å‚«‚³‚ðŠg‘å
    //        Instance._bulkHPBar.transform.DOScale(5, 0.1f);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.Alpha5))
    //    {
    //        PlayerState.playerState.defenceState = PlayerState.DefenceState.green;
    //        _sePlay.Play("CHANGE_SE_DEFENCE");
    //        Instance._bulkHPBar.transform.DOScale(5, 0.1f);
    //        //Instance._bulkHPBar.transform.DOScale(1, 1f);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.Alpha6))
    //    {
    //        PlayerState.playerState.defenceState = PlayerState.DefenceState.blue;
    //        _sePlay.Play("CHANGE_SE_DEFENCE");
    //        Instance._bulkHPBar.transform.DOScale(5, 0.1f);
    //        //Instance._bulkHPBar.transform.DOScale(1, 1f);
    //    }
    //}

    //void RenderHPColor(PlayerState.DefenceState defenceState)
    //{
    //    switch (defenceState)
    //    {
    //        case PlayerState.DefenceState.red:
    //            _defenceState.color = Color.red;
    //            Instance._bulkHPBar.transform.DOScale(1, 0.3f);
    //            break;

    //        case PlayerState.DefenceState.green:
    //            _defenceState.color = Color.green;
    //            Instance._bulkHPBar.transform.DOScale(1, 0.3f);
    //            break;

    //        case PlayerState.DefenceState.blue:
    //            _defenceState.color = Color.blue;
    //            Instance._bulkHPBar.transform.DOScale(1, 0.3f);
    //            break;
    //    }
    //}
}