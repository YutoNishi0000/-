using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetEnemyAttack : MonoBehaviour
{
    public bool IsGetHit = false;                   //�_���[�W������������ǂ���

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Attack2"))
        {
            IsGetHit = true;
            Debug.Log("�G����U�����󂯂܂���");
        }
    }
}
