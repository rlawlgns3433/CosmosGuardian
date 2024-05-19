using UnityEngine;

public class UiCameraLookAt : MonoBehaviour
{
    void LateUpdate()
    {
        // UI 오브젝트가 항상 카메라를 바라보게 설정
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                          Camera.main.transform.rotation * Vector3.up);
    }
}
