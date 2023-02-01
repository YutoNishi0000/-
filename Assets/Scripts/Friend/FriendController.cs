using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using static EnemyController;

//�����ƂȂ�L�����N�^�[�𐧌䂷��N���X
public class FriendController : Actor, IDamageable
{
    private bool _isGround;   //�n�ʂɂ��Ă��邩
    private bool _isDropped;
    public bool _isEnemy;    //�G���߂��ɂ��邩�ǂ���
    public Slider _bulkHPBar;
    public Slider _HPBar;
    [SerializeField] private int _friendHP = 100000;
    private Animator anim;
    [SerializeField] private float _coolTime;
    public Collider swordCol;
    private bool _isDead;

    // Start is called before the first frame update
    void Start()
    {
        _isDead = false;
        _isGround = false;
        _isDropped = false;
        _isEnemy = false;
        anim = GetComponent<Animator>();
        _coolTime = 3.0f;
        swordCol.enabled = false;
        _friendHP = 100000;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isDropped)
        {
            Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z + 10);
            transform.position = Camera.main.ScreenToWorldPoint(mousePos);
        }

        if(Input.GetMouseButtonDown(0))
        {
            _isDropped = true;
            Instance._isDropping = false;
            GetComponent<Rigidbody>().isKinematic = false;
        }

        Attack();
        Death();
        _isEnemy= false;
    }

    //�U������
    public override void Attack()
    {
        //�G�l�~�[���߂��ɋ�����
        if(_isEnemy)
        {
            //�U���J�n
            StartCoroutine(CAttack());
        }
        else
        {
            return;
        }
    }

    IEnumerator CAttack()
    {
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(_coolTime); 
    }

    //�_���[�W�����̏ڍ�
    public void Damage(int damegeVal)
    {
        //�_���[�W����
        _friendHP -= damegeVal;

        //HP����C�Ɍ��炷
        _bulkHPBar.value -= damegeVal;

        //DOTween���g���Ċ��炩��HP�����炵�Ă���
        _HPBar.DOValue(_bulkHPBar.value, 1f);
    }

    public void Death()
    {
        //���S��������������̂�Y��Ȃ�
        if (_friendHP <= 0)
        {
            //�v���C���[�̂Ƃ���ɖ߂�
            transform.position = Vector3.MoveTowards(transform.position, Instance.transform.position, 0.1f);
            _isDead = true;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Player") && _isDead)
        {
            //gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //�������v���C���[�̖�����������
        if (other.gameObject.CompareTag("Enemy"))
        {
            _isEnemy = true;    //�G�l�~�[���߂��ɂ���Ƃ����t���O���I���ɂ���
            EnemyController enemy = other.GetComponent<EnemyController>();
            //�t���O���I���ɂ���
            enemy._isfriend = true;
            //���x��0�ɂ���
            enemy.navMeshAgent.speed = 0.0f;
            //�ړI�n�����̃L�����N�^�[�ɂ���
            enemy.navMeshAgent.destination = transform.position;
            //�U������
            enemy._enemyState = EnemyState.Attack;
            //�G�̕���������Ɍ���������悤�ɂ���
            CorrectRotation(enemy.gameObject.transform.position, transform.position, 0.3f);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            _isEnemy = false;
        }
    }

    //�U���A�j���[�V�������n�߂鎞�ɌĂяo��
    public void StartAttack()
    {
        swordCol.enabled = true;
    }

    //�U���A�j���[�V�������I���Ƃ��ɌĂяo��
    public void EndAttack()
    {
        swordCol.enabled = false;
    }
}