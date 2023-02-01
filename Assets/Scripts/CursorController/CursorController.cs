using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerState.playerState.modeSelection != PlayerState.ModeSelection.Title)
        {
            if (Input.GetMouseButton(1))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            else if (Input.GetMouseButtonUp(1))
            {
                Cursor.lockState = CursorLockMode.None;
            }
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
