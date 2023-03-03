using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//���@�I�u�W�F�N�g�̈ړ��𐧌䂷��N���X
public class MagicObj : Actor
{
    public GameObject _explosion;

    public PlayerState.FireBall fireType;

    public int _attack;

    public AudioClip audioExplosion;


    [SerializeField, Min(0)] float time = 1;
    [SerializeField, Min(0)] float maxAcceleration = 100;
    [SerializeField] private Transform target;
    [SerializeField] private float lifeTime = 2;
    [SerializeField] private bool limitAcceleration = false;
    [SerializeField] private Vector3 minInitVelocity;
    [SerializeField] private Vector3 maxInitVelocity;
    private Vector3 position;
    private Vector3 velocity;
    private Vector3 acceleration;
    private Transform thisTransform;

    public Transform Target
    {
        set{ target = value; }
        get{ return target; }
    }

    private void Start()
    {
        _attack = PlayerState.playerState._offensivePower;

        fireType = PlayerState.playerState.fireBall;

        thisTransform = transform;
        position = thisTransform.position;
        velocity = new Vector3(Random.Range(minInitVelocity.x, maxInitVelocity.x), Random.Range(minInitVelocity.y, maxInitVelocity.y), Random.Range(minInitVelocity.z, maxInitVelocity.z));
    }

    private void Update()
    {
        if (target == null)
        {
            GetComponent<Rigidbody>().AddForce(transform.forward, ForceMode.Impulse);
            return;
        }

        //�����x�����߂�
        acceleration = 2f / (time * time) * (target.position - position - time * velocity);
        if (acceleration.sqrMagnitude > maxAcceleration * maxAcceleration)
        {
            acceleration = acceleration.normalized * maxAcceleration;
        }

        //���x���擾
        velocity += acceleration * Time.deltaTime;

        //���W���擾 
        position += velocity * Time.deltaTime;

        //���ʓI�ɋ��߂����W��������
        transform.position = position;

        //�����͐i��ł������������
        transform.rotation = Quaternion.LookRotation(velocity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Magic"))
        {
            return;
        }

        AudioSource.PlayClipAtPoint(audioExplosion, Camera.main.transform.position);
        Instantiate(_explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);

        //�G�l�~�[������������
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("���ɂ��U������������");

            IEnemyDamagable _enemyCon = collision.gameObject.GetComponent<IEnemyDamagable>();

            //�_���[�W�ʂ��v�Z
            int _damageVal = CalcDamage(_attack, fireType, collision.gameObject.GetComponent<EnemyController>()._defenceState);

            //��قǎ擾�����_���[�W�ʂ��C���^�[�t�F�[�X�̈����ɓn���A�_���[�W�������s��
            _enemyCon.Damage(_damageVal);
        }
    }

    //�_���[�W�{�����v�Z���邽�߂̊֐�
    public int CalcDamage(int DamageVal, PlayerState.FireBall fireType, EnemyController.DefenceState defenceState)
    {
        //������������������
        if((int)fireType == (int)defenceState)
        {
            //��b�U���͂̓��{�̒l��Ԃ�
            return DamageVal;
        }
        //�U���^�C�v���G�̑����Ɏ���Ď�_��������
        else if(fireType == PlayerState.FireBall.red && defenceState == EnemyController.DefenceState.green
            || fireType == PlayerState.FireBall.green && defenceState == EnemyController.DefenceState.blue
            || fireType == PlayerState.FireBall.blue && defenceState == EnemyController.DefenceState.red)
        {
            //��b�U���͂̓�{�̒l��Ԃ�
            return DamageVal * 2;
        }
        //�U���^�C�v���G�̑����Ɍ����Ȃ�������
        else
        {
            //��b�U���͂̂Q���̂P�̒l��Ԃ�
            return DamageVal / 2;
        }
    }
}