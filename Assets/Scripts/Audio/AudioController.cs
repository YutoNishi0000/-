using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (PlayerState.playerState._gameState)
        {
            case PlayerState.GemeState.GameOver:
                audioSource.Stop();
                break;
            case PlayerState.GemeState.GameClear:
                audioSource.Stop();
                break;
        }
    }
}
