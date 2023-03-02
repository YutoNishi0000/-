using UnityEngine;
using UnityEngine.UI;

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
    private SePlay _sePlay;
    private bool _damageSE = false;
    private bool _isGround = false;
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
        Move();

        Debug.Log("HP" + PlayerState.playerState.PlayerHP);
    }

    //�n�ʂƂ̐ڐG����𕨗����Z�̃t���[�����[�N�ōs���Ă��邽�ߍ��킹����
    private void FixedUpdate()
    {
    }

    public override void Move()
    {
        //�i�s�����v�Z
        //�L�[�{�[�h���͂��擾
        float v = Input.GetAxisRaw("Vertical");         //InputManager�́����̓���       
        float h = Input.GetAxisRaw("Horizontal");       //InputManager�́����̓��� 

        //�J�����̐��ʕ����x�N�g������Y�����������A���K�����ăL����������������擾
        Vector3 forward = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 right = Camera.main.transform.right; //�J�����̉E�������擾

        switch (PlayerState.playerState.state)
        {
            case PlayerState.State.None:
                //�L�[���͂��������ꍇ����������
                MoveVector = h * right + v * forward;
                break;

            case PlayerState.State.Attack:
                //�U�����͈ړ������Ȃ�������
                MoveVector = Vector3.zero;
                break;
        }

        //�J�����̐��ʌ����ɑ΂��ăL�[���͂��Ƃɉ�]��������
        RotationControl(MoveVector);

        //�Ζʂɉ������x�N�g�����擾
        Vector3 OnPlane = Vector3.ProjectOnPlane(MoveVector, NormalVector);

        //��������
        if (!_isGround)
        {
            totalFallTime += Time.deltaTime;
            OnPlane.y = Physics.gravity.y * totalFallTime;
        }
        else
        {
            totalFallTime = 0f;
        }

        //�Q�[���N���A�A�܂��̓Q�[���I�[�o�[���͈ړ��ł��Ȃ��悤�ɂ���
        if (PlayerState.playerState._gameState == PlayerState.GemeState.None)
        {
            rb.velocity = OnPlane * param._moveSpeed;         //�������x
        }
        else
        {
            rb.velocity = Vector3.zero;
        }

        AnimationManager(rb);
    }

    // �v���C���[�̉�]����
    void RotationControl(Vector3 moveVec)  //�L�����N�^�[���ړ�������ς���Ƃ��̏���
    {
        Vector3 rotateDirection = moveVec;
        rotateDirection.y = 0;

        //����Ȃ�Ɉړ��������ω�����ꍇ�݈̂ړ�������ς���
        if (rotateDirection.sqrMagnitude > 0.01)
        {
            //�ɂ₩�Ɉړ�������ς���
            float step =  300 * Time.deltaTime;
            Vector3 newDir = Vector3.Slerp(transform.forward, rotateDirection, step);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
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
    public void AnimationManager(Rigidbody rb)
    {
        //��ɃA�j���[�V�����Ǘ����s���֐�
        switch (PlayerState.playerState.state)
        {
            �@�@//�U������Ƃ�
            case PlayerState.State.Attack:
                animator.SetBool("Attack", true);
                break;

            �@�@//�����Ȃ����͕������A�C�h�����[�W�������Đ�����
            case PlayerState.State.None:
                animator.SetBool("Attack", false);
                animator.SetBool("Damage", false);

                if (rb.velocity.magnitude > 0)
                {
                    //�A�j���[�V�������o��
                    animator.SetFloat("Walk", rb.velocity.magnitude);
                }
                else
                {
                    //����������Ă��Ȃ�������ړ����[�V�������~�߂�0��Ԃ�
                    animator.SetFloat("Walk", rb.velocity.magnitude);
                }
                break;
        }
    }

    public void Death()
    {
        //���S��������������̂�Y��Ȃ�
        if(PlayerState.playerState.PlayerHP <= 0)
        {
            PlayerState.playerState._gameState = PlayerState.GemeState.GameOver;
        }
    }
}