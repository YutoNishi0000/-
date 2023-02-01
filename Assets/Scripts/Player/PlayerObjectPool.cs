using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

//�v���C���[�����I�u�W�F�N�g���I�u�W�F�N�g�v�[���ɂ���Đ��䂷��
public class PlayerObjectPool : MonoBehaviour
{
    //���˂���I�u�W�F�N�g(1�ԖڂɐԁA2�Ԗڂɗ΁A3�Ԗڂɐ�����)
    public GameObject fireObj;
    //���˂���I�u�W�F�N�g�����炩���ߊi�[���郊�X�g�^�̑������z��
    private List<GameObject> poolObjects = new List<GameObject>();
    //�ǂ̃Q�[���I�u�W�F�N�g�����ɐ������邩
    public GameObject ObjectPool;

    // Start is called before the first frame update
    void Start()
    {
        //for (int j = 0; j < fireObj.Length; j++)
        //{
            for (int i = 0; i < 20; i++)
            {
                GameObject obj = Instantiate(fireObj, ObjectPool.transform);
                obj.SetActive(false);
                poolObjects.Add(obj);
            }
        //}
    }

    /// <summary>
    /// ���˂���I�u�W�F�N�g���擾
    /// </summary>
    /// <param name="fireType">
    /// ���@����
    /// </param>
    /// <returns></returns>
    public GameObject GetPlayerFireObjects(PlayerState.FireBall fireType)
    {
        //Debug.Log(fireObj[(int)fireType]);
        //�I�u�W�F�N�g�v�[���̒������A�N�e�B�u�̃I�u�W�F�N�g��Ԃ�
        for (int i = 0; i < poolObjects.Count; i++)
        {
            //FireBall�^�̕ϐ���int�^�ɃL���X�g
            if (poolObjects[i].activeInHierarchy == false/* && poolObjects[i] == fireObj[(int)fireType]*/)
            {
                return poolObjects[i];
            }
        }

        return null;
    }
}
