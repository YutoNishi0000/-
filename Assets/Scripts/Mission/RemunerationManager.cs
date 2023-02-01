using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

//報酬を表示するためのクラス
public class RemunerationManager : MonoBehaviour
{
    private MissionManager _mission;

    private float sec;
    public bool _moveLock;
    public Image _friendPanel;
    public Image _multiburstPanel;
    private bool _moveLeft;
    public bool _multiburst;

    // Start is called before the first frame update
    void Start()
    {
        sec = 0;
        _multiburst = false;
        _moveLock = true;
        _moveLeft = true;
        _mission = GetComponent<MissionManager>();
        Debug.Log("クリア報酬出動");
    }

    // Update is called once per frame
    void Update()
    {
        switch (_moveLeft)
        {
            case true:
                MoveLeft(_friendPanel); 
                break;

            case false:
                MoveRight(_friendPanel); 
                break;
        }

        if(PlayerState.playerState._burstState == PlayerState.BurstState.Multi)
        {
            MoveLeft(_multiburstPanel);
        }
    }

    void MoveLeft(Image _panel)
    {
        if(_moveLock)
        {
            return;
        }

        _panel.transform.position -= new Vector3(0.8f, 0, 0);

        if (_panel.transform.position.x <= _mission._endPos.transform.position.x)
        {
            _panel.transform.position = new Vector3(_mission._endPos.transform.position.x, _panel.transform.position.y, _panel.transform.position.z);

            _moveLeft = false;
        }
    }

    void MoveRight(Image _panel)
    {
        sec += Time.deltaTime;

        if (sec > 2.0f)
        {
            _panel.transform.position += new Vector3(1.6f, 0, 0);

            if (_panel.transform.position.x >= _mission._startPos.transform.position.x)
            {
                sec = 0;
                _moveLock = true;
                _moveLeft = true;
            }
        }
    }
}