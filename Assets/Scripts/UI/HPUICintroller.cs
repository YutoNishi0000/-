using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUICintroller : MonoBehaviour
{
    //HPUI��]����ɃJ�����̉�]�Ɠ����ɂ���
    private void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}