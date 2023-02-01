using DG.Tweening;
using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

public class MoveController : Actor
{
    private Vector3 NormalVector;  //�R���W�����ɐڐG���Ă���n�ʂ̖@���x�N�g�����i�[
    //Animator������
    public Animator animator;
    //Main Camera������
    public Transform cam;
    public GameObject FirePos;   //���@�I�u�W�F�N�g�𐶂ݏo�����߂̈ʒu
    [System.NonSerialized] public Detection dec;             //Detection�N���X�̕ϐ����擾
    private Vector3 MoveVector;                     //�ړ��������i�[����x�N�g��
    private float _revivalTime;                      //�����܂ł̎��Ԃ��i�[����
    public PlayerAnimationEvent animEve;
    private GetEnemyAttack _getDamage;
    public PlayerParameter param;
    public Slider _bulkHPBar;
    public Slider _HPBar;
    private Rigidbody rb;
    private float totalFallTime = 0f;
    private bool _isGround = false;
    private SePlay _sePlay;
    private bool _damageSE = false;
    public bool _isDropping;       //����������������\���t���O

    // Start is called before the first frame update
    void Start()
    {
        //Animator�R���|�[�l���g���擾
        animator = GetComponent<Animator>();

        //Detection�N���X�C���X�^���X���擾
        dec = GetComponentInChildren<Detection>();

        //�v���C���[�̓����Ԃ�������
        PlayerState.playerState.state = PlayerState.State.None;

        //�v���C���[�̍U�����@������ԑ����ɂ���
        PlayerState.playerState.fireBall = PlayerState.FireBall.red;

        //�A�j���[�V�����C�x���g�Ǘ��N���X�̃C���X�^���X���擾
        animEve = GetComponentInChildren<PlayerAnimationEvent>();

        //�_���[�W������������ǂ������Ǘ�����N���X�̃C���X�^���X���擾
        _getDamage = GetComponentInChildren<GetEnemyAttack>();

        rb = GetComponent<Rigidbody>();

        _bulkHPBar.value = PlayerState.playerState.PlayerMAXHP;

        _HPBar.value = PlayerState.playerState.PlayerMAXHP;

        _sePlay = GameObject.Find("AudioManager").GetComponent<SePlay>();

        //�Q�[���J�n���ƂɓG�̌��j��������������
        PlayerState.playerState.NumCrushingEnemies = 0;

        //�Q�[���J�n���̓v���C���[��HP��������
        PlayerState.playerState.PlayerHP = PlayerState.playerState.PlayerMAXHP;

        _isDropping = false;
    }

    // Update is called once per frame
    void Update()
    {
        Death();
        AnimationManager();
        //Heal();

        Debug.Log("HP" + PlayerState.playerState.PlayerHP);
    }

    //�n�ʂƂ̐ڐG����𕨗����Z�̃t���[�����[�N�ōs���Ă��邽�ߍ��킹����
    private void FixedUpdate()
    {
        Move();
    }

    public override void Move()
    {
        //x, z ���ʂł̈ړ�
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        //Vector3 target_dir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        //�O�ړ��̎��ōU�����ȊO�̎��̓J�����̑O��������������
        if (z > 0 && PlayerState.playerState.state != PlayerState.State.Attack)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(new Vector3(transform.rotation.x, cam.eulerAngles.y, transform.rotation.z)), 3.0f);
        }

        ////�J�����̐��ʌ����ɑ΂��ăL�[���͂��Ƃɉ�]��������
        Rotation(x, z);
        ////transform.position += transform.forward * z + transform.right * x;

        switch (PlayerState.playerState.state)
        {
            case PlayerState.State.None:
                //�L�[���͂��������ꍇ����������
                MoveVector = transform.forward * GetKeyInput(x, z);/* + transform.right * x*/;
                break;

            case PlayerState.State.Attack:
                //�U�����͈ړ������Ȃ�������
                MoveVector = Vector3.zero;
                break;

            case PlayerState.State.GetDamage:
                //�ړ��ł��Ȃ��悤�ɂ���
                break;
        }

        //�}�ȍ⓹�ł��o���悤�ɂ��邽�ߒn�ʂ��瓾���@���x�N�g���Ɠ��͕����ɑ΂���x�N�g�����g���ĎΖʂɕ��s�ȃx�N�g���𓾂�
        //�n�ʂ��瓾��@���x�N�g���̓R���W�������g�����߂��̊֐���FixedUpdate�Ŏ��s����

        //�Ζʂɉ������x�N�g�����擾
        Vector3 OnPlane = Vector3.ProjectOnPlane(MoveVector, NormalVector);
        //Vector3 OnPlane = MoveVector - NormalVector;

        if (!_isGround)
        {
            totalFallTime += Time.deltaTime;
            OnPlane.y = Physics.gravity.y * totalFallTime;
            Debug.Log("�n�ʂɂ��Ă��Ȃ���[�|�|�|�|�|�|�|�|");
        }
        else
        {
            totalFallTime = 0f;
            Debug.Log("���߂�ɂ��Ă����[�|�|�|�|�|�|");
        }

        //�Q�[���N���A�A�܂��̓Q�[���I�[�o�[���͈ړ��ł��Ȃ��悤�ɂ���
        if (PlayerState.playerState._gameState != PlayerState.GemeState.GameClear && PlayerState.playerState._gameState != PlayerState.GemeState.GameOver)
        {
            rb.velocity = OnPlane * Avoidance(param._moveSpeed, param._moveSpeed * 3f);         //�������x
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

        ////�Ζʂɉ������x�N�g�����g���Ĉړ�������
        //transform.position += /*param._moveSpeed*/Avoidance(param._moveSpeed, param._moveSpeed * 1.5f) * Time.deltaTime * OnPlane;/*transform.forward * z + transform.right * x*/;
    }

    //�i�s�����ɑ΂��ăv���C���[�I�u�W�F�N�g�̉�]��������֐�
    void Rotation(float AxisX, float AxisZ)
    {
        //�����ł̓v���C���[�̉�]��������ɍs���֐�

        //�����̈ړ��̎��������̊֐������s����i�U�����͎��s���Ȃ��j

        if (PlayerState.playerState.state == PlayerState.State.None)
        {
            //�E����
            if (AxisX > 0)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, cam.eulerAngles.y + 90, transform.rotation.z), 5.0f);
            }
            //������
            if (AxisX < 0)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, cam.eulerAngles.y - 90, transform.rotation.z), 5.0f);
            }
            //�����
            if (AxisZ > 0)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, cam.eulerAngles.y + 0, transform.rotation.z), 5.0f);
            }
            //������
            if (AxisZ < 0)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(transform.rotation.x, cam.eulerAngles.y + 180, transform.rotation.z), 7.0f);
            }
        }
    }

    //�L�[�̓��͂��Ȃ��ꂽ��float�^�̒l(1)��Ԃ��֐�
    float GetKeyInput(float AxisX, float AxisZ)
    {
        if(AxisX != 0 || AxisZ != 0)
        {
            //�A�j���[�V�������o��
            animator.SetBool("Walk", true);
            return 1.0f;
        }
        else
        {
            //����������Ă��Ȃ�������ړ����[�V�������~�߂�0��Ԃ�
            animator.SetBool("Walk", false);
            return 0.0f;
        }
    }

    //������s���֐�
    float Avoidance(float speed, float acceleration)
    {
        //if(Input.GetMouseButton(1))
        //{
        //    return acceleration;
        //}
        //else if(Input.GetMouseButtonUp(1))
        //{
        //    float movespeed = Mathf.MoveTowards(acceleration, speed, Time.deltaTime);

        //    return movespeed;
        //}
        //else
        //{
        //    return speed;
        //}
        return speed;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // �Փ˂����ʂ́A�ڐG�����_�ɂ�����@�����擾
            NormalVector = collision.GetContact(0).normal;
            _isGround = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _isGround = false;
        }
    }

    //��ɃA�j���[�V�������Ǘ�����֐��i���s���ȊO�j
    public override void AnimationManager()
    {
        //��ɃA�j���[�V�����Ǘ����s���֐�
        switch (PlayerState.playerState.state)
        {
            �@�@//�U������Ƃ�
            case PlayerState.State.Attack:
                animator.SetBool("Attack", true);
                break;

            //�@�@//�_���[�W���󂯂��Ƃ�
            //case PlayerState.State.GetDamage:
            //    animator.SetBool("Damage", true);
            //    Debug.Log("�G����_���[�W���������");
            //    break;

            �@�@//�����Ȃ����͕������A�C�h�����[�W�������Đ�����
            case PlayerState.State.None:
                animator.SetBool("Attack", false);
                animator.SetBool("Damage", false);
                break;
        }
    }

    //public override void GetDamage()
    //{
    //    //if (_getDamage.IsGetHit)
    //    //{
    //    //    if (!_damageSE)
    //    //    {
    //    //        _sePlay.Play("GET_DAMAGE");
    //    //        _damageSE = true;
    //    //    }

    //    //    //���A����܂ł̎��Ԃ��v��
    //    //    _revivalTime += Time.deltaTime;

    //    //    //�v���C���[�X�e�[�g��GetDamage��
    //    //    PlayerState.playerState.state = PlayerState.State.GetDamage;

    //    //    if(_revivalTime > param._revivalTime)
    //    //    {
    //    //        //���������A�ł���悤�ɂȂ�����X�e�[�g��None�ɖ߂�
    //    //        _revivalTime = 0;
    //    //        PlayerState.playerState.state = PlayerState.State.None;
    //    //        _getDamage.IsGetHit = false;
    //    //        _damageSE = false;
    //    //    }
    //    //}
    //}

    //�_���[�W�����̏ڍ�
    //public void Damage(int damegeVal)
    //{
    //    //�_���[�W����
    //    PlayerState.playerState.PlayerHP -= damegeVal;

    //    //HP����C�Ɍ��炷
    //    _bulkHPBar.value -= damegeVal;

    //    //DOTween���g���Ċ��炩��HP�����炵�Ă���
    //    _HPBar.DOValue(_bulkHPBar.value, 1f);
    //}

    public void Death()
    {
        //���S��������������̂�Y��Ȃ�
        if(PlayerState.playerState.PlayerHP <= 0)
        {
            PlayerState.playerState._gameState = PlayerState.GemeState.GameOver;
        }
    }
}

//�I�u�W�F�N�g���N���X
public class Actor : MonoBehaviour
{
    //PlayerController�^�̕ϐ��錾
    protected MoveController Instance;

    private void Awake()
    {
        //PlayerController�^�̃C���X�^���X���擾
        Instance = FindObjectOfType<MoveController>();
    }

    public virtual void Move() { }  //�ړ��p���z�֐�

    public virtual void Attack() { }  //�U���p���z�֐�

    public virtual void GetDamage() { }  //��_���p���z�֐�

    /// <summary>
    /// ���������������Ɋ��炩�ɉ�]�����邽�߂̊֐�(�ǂ̃N���X�ł��悭�g�����߂����ɌĂяo����悤�ɂ���)
    /// </summary>
    /// <param name="directon">
    /// �������������ł���^�[�Q�b�g�̈ʒu
    /// </param>
    /// <param name="myPos">
    /// �����̈ʒu
    /// </param>
    /// <param name="sec">
    /// ��]��ԃX�s�[�h
    /// </param>
    public void CorrectRotation(Vector3 directon, Vector3 myPos, float sec)
    {
        var targetRotation = Quaternion.LookRotation(directon - myPos);
        targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, sec);
    }

    public virtual void AnimationManager() { }
}

//�_���[�W�����C���^�[�t�F�C�X(�v���C���[�p)
public interface IDamageable
{
    //�_���[�W����������Ƃ��̏���
    public void Damage(int damegeVal);

    //���S����
    public void Death();
}

//�o���l�C���^�[�t�F�C�X
public interface IExp
{
    public void LevelUp();

    public void GetExp(int exp);
}