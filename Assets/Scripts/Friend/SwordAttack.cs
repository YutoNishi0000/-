using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : Actor
{
    [SerializeField] private int _attackPower;
    private FriendController _friendController;
    [SerializeField] private GameObject _explosion;
    [SerializeField] private AudioClip audioExplosion;

    // Start is called before the first frame update
    void Start()
    {
        _attackPower = 2000;
    }

    private void OnTriggerEnter(Collider other)
    {
        AudioSource.PlayClipAtPoint(audioExplosion, Camera.main.transform.position);
        Instantiate(_explosion, transform.position, Quaternion.identity);

        //エネミーが当たったら
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("剣による攻撃が当たった");

            IEnemyDamagable _enemyCon = other.gameObject.GetComponent<IEnemyDamagable>();

            _enemyCon.Damage(_attackPower);
        }
    }
}
