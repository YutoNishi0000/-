using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

//�͈͓��ɓ������G�̈ʒu�����擾����
public class Detection : MonoBehaviour
{
    private Vector3 EnemyPosition;    //�G�l�~�[�̈ʒu�����i�[����ϐ�
    public bool EnemyDetection;       //�G�l�~�[�����邩�ǂ���
    private GameObject[] detectObjects;   //�t�B�[���h���ɂ���G�l�~�[�̃I�u�W�F�N�g���i�[���邽�߂̕ϐ�

    private void Start()
    {
        EnemyDetection = false;
    }

    //���̊֐����Ăяo���ꂽ�������G�l�~�[�̈ʒu�����擾����
    public Vector3 GetEnemyPos()
    {
        if (detectObjects != null)
        {
            //�G�l�~�[�̈ʒu�����擾
            SearchEnemy();
        }
        return EnemyPosition;
    }

    void SearchEnemy()
    {
        //����̃^�O�̃Q�[���I�u�W�F�N�g���i�[
        detectObjects = GameObject.FindGameObjectsWithTag("Enemy");

        //�����l�̐ݒ�
        float closeDist = 1000;

        foreach (GameObject t in detectObjects)
        {
            //�擾�����I�u�W�F�N�g�Ƃ̋������v������
            float tDist = Vector3.Distance(transform.position, t.transform.position);

            //���������l�����v�������G�܂ł̋������߂����
            if (closeDist > tDist)
            {
                //���g�ƓG�Ƃ̋������㏑������
                closeDist = tDist;

                //�G�l�~�[�̏ꏊ���i�[
                EnemyPosition = new Vector3(t.transform.position.x, t.transform.position.y + 0.5f, t.transform.position.z);

                Debug.Log("��ԋ߂��G�̃I�u�W�F�N�g���擾");
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            //�͈͓��ɃG�l�~�[��������
            EnemyDetection = true;
            Debug.Log("�G���i�����Ă���");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            //�͈͂���G�l�~�[�����Ȃ��Ȃ�����
            EnemyDetection = false;
        }
    }
}
