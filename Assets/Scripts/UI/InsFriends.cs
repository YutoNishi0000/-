using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ボタンを押したら味方キャラを生成
public class InsFriends : Actor
{
    public GameObject friend;

    private void Update()
    {
        InstantiateFriend();
    }

    //見方を召喚する関数
    public void InstantiateFriend()
    {
        if (Input.GetKeyDown(KeyCode.E) && PlayerState.playerState.NumFriends > 0)
        {
            Debug.Log("友達召喚！");
            Instance._isDropping = true;
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z + 10));
            Instantiate(friend, mousePos, Quaternion.identity);
            PlayerState.playerState.NumFriends--;
        }
    }
}