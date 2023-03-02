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
            Debug.Log("�F�B�����I");
            Instance._isDropping = true;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z + 10));
            Instantiate(friend, mousePos, Quaternion.identity);
            PlayerState.playerState.NumFriends--;
        }
    }
}