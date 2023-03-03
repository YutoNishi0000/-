using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxisController : Actor
{
    //X���̊p�x�𐧌����邽�߂̕ϐ�
    float angleUp = 60f;
    float angleDown = -60f;

    //�v���C���[��Inspector�œ����
    [SerializeField] GameObject player;
    //Main Camera��Inspector�œ����
    [SerializeField] Camera cam;

    //Camera����]����X�s�[�h
    [SerializeField] float rotate_speed = 3;
    //Axis�̈ʒu���w�肷��ϐ�
    [SerializeField] Vector3 axisPos;

    //�}�E�X�X�N���[���̒l������
    [SerializeField] float scroll;
    //�}�E�X�z�C�[���̒l��ۑ�
    [SerializeField] float scrollLog;

    [SerializeField] Vector3 offSet;

    float time;
    float sec;

    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Camera��Axis�ɑ��ΓI�Ȉʒu��localPosition�Ŏw��
        cam.transform.localPosition = new Vector3(0, 0, -3);
        //Camera��Axis�̌������ŏ��������낦��
        cam.transform.localRotation = transform.rotation;
        //�J�����̈ʒu��������
        cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, cam.transform.localPosition.z - 5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Move();
        }

        //Axis�̈ʒu���v���C���[�̈ʒu�{axisPos�Ō��߂�
        transform.position = player.transform.position + axisPos;
    }

    //�J�����̓��쏈��
    public override void Move()
    {
        //�}�E�X�X�N���[���̒l������
        scroll = Input.GetAxis("Mouse ScrollWheel");

        //�}�E�X�X�N���[���̒l�͓������Ȃ���0�ɂȂ�̂ł����ŕۑ�����
        scrollLog += Input.GetAxis("Mouse ScrollWheel");

        //Camera�̈ʒu�AZ���ɃX�N���[������������
        cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, cam.transform.localPosition.z + scroll);

        //Camera�̊p�x�Ƀ}�E�X����Ƃ����l������
        transform.eulerAngles += new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X") * rotate_speed, 0);

        //X���̊p�x
        float angleX = transform.eulerAngles.x;

        //X���̒l��180�x��������360�������ƂŐ������₷������
        if (angleX >= 180)
        {
            angleX -= 360;
        }

        //Mathf.Clamp(�l�A�ŏ��l�A�ő�l�j��X���̒l�𐧌�����
        transform.eulerAngles = new Vector3(Mathf.Clamp(angleX, angleDown, angleUp), transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
