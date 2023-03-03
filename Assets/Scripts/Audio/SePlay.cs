using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SePlay : MonoBehaviour
{
    [SerializeField]
    AudioClip[] audioClips;

    AudioSource audioSource;

    private void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();
    }

    //���ʉ����Đ�����
    public void Play(string seName)
    {
        switch (seName)
        {
            case "Button":
                audioSource.PlayOneShot(audioClips[0]);
                break;
            case "Attack":
                audioSource.PlayOneShot(audioClips[1]);
                break;
            case "GAMEOVER":
                audioSource.PlayOneShot(audioClips[2]);
                break;
            case "GAMECLEAR":
                audioSource.PlayOneShot(audioClips[3]);
                break;
            case "CHANGE_SE_ATTACK":
                audioSource.PlayOneShot(audioClips[4]);
                break;
            case "CHANGE_SE_DEFENCE":
                audioSource.PlayOneShot(audioClips[5]);
                break;
            case "HEAL_APPLE":
                audioSource.PlayOneShot(audioClips[6]);
                break;
            case "GET_DAMAGE":
                audioSource.PlayOneShot(audioClips[7]);
                break;
        }
    }
}