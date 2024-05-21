using UnityEngine;

public class BgmManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip[] bgmClips;

    // Start is called before the first frame update
    void Start()
    {
        audioSource.volume = ParamManager.BgmValue;
        audioSource.clip = bgmClips[0];
        audioSource.loop = true;
        audioSource.Play();
    }
}
