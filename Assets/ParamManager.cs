using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParamManager : MonoBehaviour
{
    public static bool IsCameraShaking = true;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
