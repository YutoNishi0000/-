using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//魔法オブジェクトの移動を制御するクラス
public class MagicObj : Actor
{
    public GameObject _explosion;

    public PlayerState.FireBall fireType;

    public int _attack;

    public AudioClip audioExplosion;


    [SerializeField]
    Transform target;
    [SerializeField, Min(0)]
    float time = 1;
    [SerializeField]
    float lifeTime = 2;
    [SerializeField]
    bool limitAcceleration = false;
    [SerializeField, Min(0)]
    float maxAcceleration = 100;
    [SerializeField]
    Vector3 minInitVelocity;
    [SerializeField]
    Vector3 maxInitVelocity;
    Vector3 position;
    Vector3 velocity;
    Vector3 acceleration;
    Transform thisTransform;

    public Transform Target
    {
        set
        {
            target = value;
        }
        get
        {
            return target;
        }
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
        acceleration = 2f / (time * time) * (target.position - position - time * velocity);
        if (acceleration.sqrMagnitude > maxAcceleration * maxAcceleration)
        {
            acceleration = acceleration.normalized * maxAcceleration;
        }
        velocity += acceleration * Time.deltaTime;
        position += velocity * Time.deltaTime;
        transform.position = position;
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

        //エネミーが当たったら
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("剣による攻撃が当たった");

            IEnemyDamagable _enemyCon = collision.gameObject.GetComponent<IEnemyDamagable>();

            //ダメージ量を計算
            int _damageVal = CalcDamage(_attack, fireType, collision.gameObject.GetComponent<EnemyController>()._defenceState);

            //先ほど取得したダメージ量をインターフェースの引数に渡し、ダメージ処理を行う
            _enemyCon.Damage(_damageVal);
        }
    }

    //ダメージ倍率を計算するための関数
    public int CalcDamage(int DamageVal, PlayerState.FireBall fireType, EnemyController.DefenceState defenceState)
    {
        //属性が同じだったら
        if((int)fireType == (int)defenceState)
        {
            //基礎攻撃力の等倍の値を返す
            return DamageVal;
        }
        //攻撃タイプが敵の属性に取って弱点だったら
        else if(fireType == PlayerState.FireBall.red && defenceState == EnemyController.DefenceState.green
            || fireType == PlayerState.FireBall.green && defenceState == EnemyController.DefenceState.blue
            || fireType == PlayerState.FireBall.blue && defenceState == EnemyController.DefenceState.red)
        {
            //基礎攻撃力の二倍の値を返す
            return DamageVal * 2;
        }
        //攻撃タイプが敵の属性に効かなかったら
        else
        {
            //基礎攻撃力の２分の１の値を返す
            return DamageVal / 2;
        }
    }
}