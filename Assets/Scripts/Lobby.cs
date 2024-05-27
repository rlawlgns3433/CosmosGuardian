using System.Collections.Generic;
using UnityEngine;

public class Lobby : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] bgmClips;

    void Start()
    {
        audioSource.volume = ParamManager.bgmValue;
        audioSource.clip = bgmClips[0];
        audioSource.loop = true;
        audioSource.Play();
    }
}
