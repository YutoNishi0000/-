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

            //damageable?.Damage(param._damage);
        }
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
        //if (PlayerState.playerState.modeSelection != PlayerState.ModeSelection.Adventure)
        {
            if (other.gameObject.CompareTag("Tree"))
            {
                //�C���^�[�t�F�C�X���Ăяo��
                ITreeController treeController = other.gameObject.GetComponent<ITreeController>();

                treeController.Damage(param._damage);
            }
        }
    }

    //int CalcDamage(int damageVal, PlayerState.DefenceState defenceState, EnemyController.DefenceState enemyState)
    //{
    //    //�v���C���[�̍U�����������g�i�G�l�~�[�j�̖h�䑮���Ɠ�����������
    //    if ((int)defenceState == (int)enemyState)
    //    {
    //        //���{
    //        int damage = FinalDamage(damageVal, PlayerState.playerState._defencePower);

    //        return damage;
    //    }
    //    //�v���C���[�̍U�����������g�i�G�l�~�[�j�̖h�䑮���ɑ΂��ċ���������
    //    else if(enemyState == EnemyController.DefenceState.red && defenceState == PlayerState.DefenceState.green
    //        || enemyState == EnemyController.DefenceState.green && defenceState == PlayerState.DefenceState.blue
    //        || enemyState == EnemyController.DefenceState.blue && defenceState == PlayerState.DefenceState.red)
    //    {
    //        //��{
    //        int damage = FinalDamage(damageVal * 2, PlayerState.playerState._defencePower);

    //        return damage;
    //    }
    //    //�v���C���[�̍U�����������g�i�G�l�~�[�j�̖h�䑮���ɑ΂��Ďォ������
    //    else
    //    {
    //        //�񕪂̈�{
    //        int damage = FinalDamage(damageVal / 2, PlayerState.playerState._defencePower);

    //        return damage;
    //    }
    //}

    //�h��͂��܂߂��ŏI�I�ȃv���C���[�̃_���[�W�l
    int FinalDamage(int damageVal, int defenceVal)
    {
        int finaldamageVal = damageVal - defenceVal;

        if(finaldamageVal <= 0)
        {
            return 0;
        }
        else
        {
            return finaldamageVal;
        }
    }
}