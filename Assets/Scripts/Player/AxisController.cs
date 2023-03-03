using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisController : Actor
{
    //X軸の角度を制限するための変数
    float angleUp = 60f;
    float angleDown = -60f;

    //プレイヤーをInspectorで入れる
    [SerializeField] GameObject player;
    //Main CameraをInspectorで入れる
    [SerializeField] Camera cam;

    //Cameraが回転するスピード
    [SerializeField] float rotate_speed = 3;
    //Axisの位置を指定する変数
    [SerializeField] Vector3 axisPos;

    //マウススクロールの値を入れる
    [SerializeField] float scroll;
    //マウスホイールの値を保存
    [SerializeField] float scrollLog;

    [SerializeField] Vector3 offSet;

    float time;
    float sec;

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //CameraのAxisに相対的な位置をlocalPositionで指定
        cam.transform.localPosition = new Vector3(0, 0, -3);
        //CameraとAxisの向きを最初だけそろえる
        cam.transform.localRotation = transform.rotation;
        //カメラの位置を初期化
        cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, cam.transform.localPosition.z - 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Move();
        }

        //Axisの位置をプレイヤーの位置＋axisPosで決める
        transform.position = player.transform.position + axisPos;
    }

    //カメラの動作処理
    public override void Move()
    {
        //マウススクロールの値を入れる
        scroll = Input.GetAxis("Mouse ScrollWheel");

        //マウススクロールの値は動かさないと0になるのでここで保存する
        scrollLog += Input.GetAxis("Mouse ScrollWheel");

        //Cameraの位置、Z軸にスクロール分を加える
        cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, cam.transform.localPosition.z + scroll);

        //Cameraの角度にマウスからとった値を入れる
        transform.eulerAngles += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X") * rotate_speed, 0);

        //X軸の角度
        float angleX = transform.eulerAngles.x;

        //X軸の値を180度超えたら360引くことで制限しやすくする
        if (angleX >= 180)
        {
            angleX -= 360;
        }

        //Mathf.Clamp(値、最小値、最大値）でX軸の値を制限する
        transform.eulerAngles = new Vector3(Mathf.Clamp(angleX, angleDown, angleUp), transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
