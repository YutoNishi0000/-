using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicAttack : Actor
{
    public GameObject[] fireBalls;
    public float speed;
    private bool _attack = false;
    private Quaternion _cameraAngle;
    private Quaternion _cameraRotation;
    private float _coolTime;           //�N�[���^�C���̎��Ԃ��i�[
    [SerializeField] private SePlay _sePlay;
    public GameObject[] _multiFirePos;
    public bool _isRayHit;  //�}�E�X�����΂���Ray���G�l�~�[�Ƀq�b�g���Ă��邩�ǂ���
    [SerializeField]
    Transform target;
    [SerializeField]
    GameObject prefab;
    [SerializeField, Min(1)]
    int iterationCount = 3;
    [SerializeField]
    float interval = 0.1f;
    bool isSpawning = false;
    Transform thisTransform;
    WaitForSeconds intervalWait;
    private bool _isCoolTime;

    // Start is called before the first frame update
    void Start()
    {
        _isCoolTime = false;
        _isRayHit = false;
        _sePlay = GameObject.Find("AudioManager").GetComponent<SePlay>();
        thisTransform = transform;
        intervalWait = new WaitForSeconds(interval);
    }

    // Update is called once per frame
    void Update()
    {
        //if (isSpawning)
        //{
        //    return;
        //}
        //StartCoroutine(nameof(SpawnMissile));

        Move();
        SwitchFireType();
        CheckEnemy();
    }

    //�}�E�X�J�[�\����ɃG�l�~�[�������ꍇ�̏������L�q
    void CheckEnemy()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Ray�𐶐�
        RaycastHit hit;
        //���C���Փ˂����I�u�W�F�N�g�̃��C���[���G�l�~�[�ȊO�̏ꍇ�̓X���[������
        int layerMask = 1 << 13;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) // Ray�𓊎�
        {
            //�������^�O���G�l�~�[��������
            if (hit.collider.CompareTag("Enemy")) // �^�O���r
            {
                //���f���̗֊s��ԐF�ŕ`�悷��
                hit.collider.gameObject.GetComponent<Outline>().OutlineColor = Color.red;

                //�����ŃN���b�N������U���������s������
                if (PlayerState.playerState._gameState != PlayerState.GemeState.GameClear && PlayerState.playerState._gameState != PlayerState.GemeState.GameOver)
                {
                    //�{�^�����������Ƃ��A�N�[���^�C�����I����ĂĖ����������łȂ����
                    if (Input.GetMouseButtonDown(0) && _coolTime <= 0 && !Instance._isDropping)
                    {
                        PlayerState.playerState.state = PlayerState.State.Attack;
                        _attack = false;
                        _isCoolTime = true;
                        _coolTime = Instance.param._coolTime;
                        //Instance.transform.rotation = Quaternion.RotateTowards(Instance.transform.rotation, _cameraRotation, 10f);
                        //_cameraAngle = Quaternion.Euler(new Vector3(Instance.transform.rotation.x, Instance.cam.eulerAngles.y, Instance.transform.rotation.z));
                        //_cameraRotation = Quaternion.Euler(Instance.cam.eulerAngles);
                    }
                }
            }
            target = hit.collider.gameObject.transform;
        }
    }

    public override void Move()
    {
        if(PlayerState.playerState.state == PlayerState.State.Attack)
        {
            //�U�����[�V�������n�܂��Ă�����A���@�U�����s��
            if (Instance.animEve._attack)
            {
                //��������@�ʂ𔭎˂ł���Ηǂ��̂�1�t���[���������U���������s��
                if (!_attack)
                {
                    _sePlay.Play("Attack");
                    Attack();
                    _attack = true;
                }
            }
            //�N���b�N���Ă���A�j���[�W�������n�܂�܂ł̊�
            else if(!Instance.animEve._attack && !_attack)
            {
                if(target == null)
                {
                    Debug.Log("�W�I�������܂���");
                    return;
                }
                //�I�������G�̕����������悤�ɂ���
                Instance.transform.rotation = Quaternion.RotateTowards(Instance.transform.rotation, Quaternion.LookRotation(target.position - Instance.transform.position), 10f);

            }

            //�U�����[�V�������I�������A�ʏ��Ԃɖ߂�
            if (Instance.animEve._endAttack)
            {
                PlayerState.playerState.state = PlayerState.State.None;
                Instance.animEve._endAttack = false;
            }
        }

        if (_isCoolTime)
        {
            //�N�[���^�C������o�ߎ��Ԃ��������ƂŒl��0�ȉ��ɂȂ�N�[���^�C�����I���d�g��
            _coolTime -= Time.deltaTime;

            if (_coolTime < 0)
            {
                //�X�^�b�N�I�[�o�[�t���[�𔭐������Ȃ����߂ɃN�[���^�C�����i�[����l��0�����������0��������
                _coolTime = 0;
                _isCoolTime = false;
            }
        }
    }

    void SwitchFireType()
    {
        //1����������ԑ���
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayerState.playerState.fireBall = PlayerState.FireBall.red;
            _sePlay.Play("CHANGE_SE_ATTACK");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayerState.playerState.fireBall = PlayerState.FireBall.green;
            _sePlay.Play("CHANGE_SE_ATTACK");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayerState.playerState.fireBall = PlayerState.FireBall.blue;
            _sePlay.Play("CHANGE_SE_ATTACK");
        }
    }

    public override void Attack()
    {
        if (PlayerState.playerState._burstState == PlayerState.BurstState.Single)
        {
            //���@�̎�ނ��w�肵�čU�����s��
            MagicObj homing;
            homing = Instantiate(fireBalls[(int)PlayerState.playerState.fireBall], transform.position, Quaternion.identity).GetComponent<MagicObj>();
            homing.Target = target;
        }
        //else if(PlayerState.playerState._burstState == PlayerState.BurstState.Multi)
        //{
        //    for(int i = 0; i < _multiFirePos.Length; i++)
        //    {
        //        //���@�̎�ނ��w�肵�čU�����s��
        //        MagicObj homing;
        //        homing = Instantiate(fireBalls[(int)PlayerState.playerState.fireBall], _multiFirePos[i].transform.position, Quaternion.identity).GetComponent<MagicObj>();
        //        homing.Target = target;
        //    }
        //}
    }
}