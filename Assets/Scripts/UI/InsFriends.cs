using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�{�^�����������疡���L�����𐶐�
public class InsFriends : Actor
{
    public GameObject friend;

    private void Update()
    {
        InstantiateFriend();
    }

    //��������������֐�
    public void InstantiateFriend()
    {
        if (Input.GetKeyDown(KeyCode.E) && PlayerState.playerState.NumFriends > 0)
        {
            //��������������t���O���I���ɂ��čU������Ƃ��Ə��������Ԃ�Ȃ��悤�ɂ���
            Instance._isDropping = true;

            //�}�E�X�J�[�\���̈ʒu�ɐ�������������z�u
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z + 10));
            Instantiate(friend, mousePos, Quaternion.identity);

            //�������Ă��閡���̐���1���炷
            PlayerState.playerState.NumFriends--;
        }
    }
}