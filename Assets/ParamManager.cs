using UnityEngine;

public class ParamManager : MonoBehaviour
{
    public static bool IsCameraShaking = true;
    public static float BgmValue = 1;
    public static float SfxValue = 1;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
