using UnityEngine;

public class StartPlatform : MonoBehaviour
{
    private void OnEnable()
    {
        Destroy(gameObject, 10);
    }
}
