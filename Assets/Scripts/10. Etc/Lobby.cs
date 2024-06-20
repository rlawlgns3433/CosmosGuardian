using UnityEngine;

public class Lobby : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] bgmClips;

    void Start()
    {
        audioSource.volume = ParamManager.BgmValue;
        audioSource.clip = bgmClips[0];
        audioSource.loop = true;
        audioSource.Play();
    }
}