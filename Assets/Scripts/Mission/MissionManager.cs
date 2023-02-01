using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.VisualScripting;

//�������l�����邽�߂̃~�b�V�������Ǘ�����N���X
public class MissionManager : MonoBehaviour
{
    public Text _MissionText;
    public Image _MissionPanel;
    private int _NumEnemies;
    public Image _startPos;
    public Image _endPos;
    private bool _panelUp;
    private bool _getTop;     //�X�^�[�g�ʒu�ɂ������ǂ���
    private bool once;
    private float sec;
    public static bool _end;
    private bool Item;
    public GameObject[] _remunerationObjcts;
    public GameObject[] _remunerationPos;
    private RemunerationManager _remunerationManager;

    // Start is called before the first frame update
    void Start()
    {
        Item = false;
        _end = false;
        sec = 0;
        once = false;
        _getTop = false;
        _panelUp = false;
        _NumEnemies = 1;
        _MissionText.text = "�G��" + _NumEnemies + "�̓|�����I";
        _MissionPanel.transform.DOMove(_endPos.transform.position, 0.5f);
        _remunerationManager = GetComponent<RemunerationManager>();
        PlayerState.playerState._burstState = PlayerState.BurstState.Single;
    }

    // Update is called once per frame
    void Update()
    {
        switch (PlayerState.playerState.NumCrushingEnemies)
        {
            case 1:
                if(!Item)
                {
                    PlayerState.playerState.NumFriends++;
                    _remunerationManager._moveLock = false;
                    Item = true;
                }
                MissionFunc(2);
                break;
            case 3:
                if(Item)
                {
                    PlayerState.playerState.NumFriends++;
                    _remunerationManager._moveLock = false;
                    Item = false;
                }
                MissionFunc(3);
                break;
            case 6:
                if (!Item)
                {
                    PlayerState.playerState.NumFriends++;
                    PlayerState.playerState._burstState = PlayerState.BurstState.Multi;
                    _remunerationManager._moveLock = false;
                    Item = true;
                }
                MissionFunc(4);
                break;
            case 10:
                if (Item)
                {
                    PlayerState.playerState.NumFriends++;
                    _remunerationManager._moveLock = false;
                    Item = false;
                }
                MissionFunc(5);
                break;
            case 15:
                if (!Item)
                {
                    PlayerState.playerState.NumFriends++;
                    _remunerationManager._moveLock = false;
                    Item = true;
                }
                MissionAllClear();
                break;
        }
    }

    void MissionFunc(int targetNumEnemies)
    {
        if(_end)
        {
            return;
        }

        if (!once)
        {
            ClearMission();

            _MissionPanel.transform.position += new Vector3(0.8f, 0, 0);
        }

        if (_MissionPanel.transform.position.x >= _startPos.transform.position.x)
        {
            ChangeMission(targetNumEnemies);
            once = true;
        }

        if (once)
        {
            _MissionPanel.transform.position -= new Vector3(0.8f, 0, 0);

            if (_MissionPanel.transform.position.x <= _endPos.transform.position.x)
            {
                _MissionPanel.transform.position = _endPos.transform.position;
                once = false;
                _end = true;
            }
        }
    }

    void ChangeMission(int NumEnemies)
    {
        _MissionText.text = "�~�b�V�����F�G��" + NumEnemies + "�̓|�����I";
    }

    void ClearMission()
    {
        _MissionText.text = "�~�b�V�����N���A�I";
    }

    void MissionAllClear()
    {
        _MissionText.text = "�S�Ẵ~�b�V�������N���A������I";

        sec += Time.deltaTime;

        if(sec > 1.0f)
        {
            _MissionText.text = "��͎c�莞�Ԑ_�؂���蔲�����I";
        }

        _MissionPanel.transform.position -= new Vector3(0, 0.3f, 0);

        if (_MissionPanel.transform.position.y <= _endPos.transform.position.y)
        {
            _MissionPanel.transform.position = _endPos.transform.position;
        }
    }
}
