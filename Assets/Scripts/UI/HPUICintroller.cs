using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPUICintroller : MonoBehaviour
{
    //HPUI回転を常にカメラの回転と同じにする
    private void LateUpdate()
    {
        transform.rotation = Camera.main.transform.rotation;
    }
}