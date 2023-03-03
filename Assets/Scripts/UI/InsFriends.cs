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
            //味方を召喚するフラグをオンにして攻撃するときと処理がかぶらないようにする
            Instance._isDropping = true;

            //マウスカーソルの位置に生成した見方を配置
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z + 10));
            Instantiate(friend, mousePos, Quaternion.identity);

            //所持している味方の数を1減らす
            PlayerState.playerState.NumFriends--;
        }
    }
}