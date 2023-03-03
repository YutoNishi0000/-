using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
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
