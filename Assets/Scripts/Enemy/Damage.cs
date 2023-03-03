using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TreeController;

//�U�����̃G�l�~�[�̓����蔻�菈��
public class Damage : Actor
{
    private EnemyParameter param;

    private void Start()
    {
        param = GetComponentInParent<EnemyController>().param;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            //�C���^�[�t�F�C�X���Ăяo��
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.Damage(param._damage);
            }
        }
        //�������U��������
        else if (other.gameObject.CompareTag("Friend"))
        {
            //�C���^�[�t�F�C�X���Ăяo��
            IDamageable damageable = other.gameObject.GetComponent<IDamageable>();

            if (damageable != null)
            {
                //�_���[�W���������ۂɍs��
                damageable.Damage(param._damage);
            }
        }
        //�؂��U��������
        else if (other.gameObject.CompareTag("Tree"))
        {
            //�C���^�[�t�F�C�X���Ăяo��
            ITreeController treeController = other.gameObject.GetComponent<ITreeController>();

            treeController.Damage(param._damage);
        }
    }
}