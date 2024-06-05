using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] bgmClips;
    public List<AudioClip> flashClips = new List<AudioClip>();
    public List<AudioClip> hitClips = new List<AudioClip>();

    void Start()
    {
        audioSource.volume = ParamManager.BgmValue;
        audioSource.clip = bgmClips[0];
        audioSource.loop = true;
        audioSource.Play();
    }
}