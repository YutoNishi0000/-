using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvent : Actor
{
    public Collider[] col;
    public bool _endAttck2 = false;
    public bool _death = false;
    private EnemyParameter param;

    private void Start()
    {
        _endAttck2 = false;
        _death = false;
        param = GetComponent<EnemyController>().param;
        for (int i = 0; i < col.Length; i++)
        {
            col[i].enabled = false;
        }
    }

    //�U���J�n���ɌĂяo���֐�
    void StartAttack2()
    {
        for(int i = 0; i < col.Length; i++)
        {
            col[i].enabled = true;
        }
        Debug.Log("�A�^�b�N�J�n");
    }

    //�U�����I�������ɌĂяo���֐�
    void EndAttack2()
    {
        for (int i = 0; i < col.Length; i++)
        {
            col[i].enabled = false;
        }
        Debug.Log("�A�^�b�N�I���");
    }

    //�A�j���[�V�������n�܂�������ɌĂяo���֐�
    void StartAnimation()
    {
        _endAttck2 = false;
    }

    //�A�j���[�V�������I�������ɌĂяo���֐�
    void EndAnimation()
    {
        _endAttck2 = true;
    }

    void Death()
    {
        _death = true;
        Debug.Log("�G�l�~�[�����񂾁I�I");
        Destroy(this.gameObject);
        PlayerState.playerState.NumCrushingEnemies++;
        MissionManager._end = false;
        Debug.Log("�G���j��" + PlayerState.playerState.NumCrushingEnemies);
    }
}