using UnityEngine;

public class UiCameraLookAt : MonoBehaviour
{
    void LateUpdate()
    {
        // UI ������Ʈ�� �׻� ī�޶� �ٶ󺸰� ����
        transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
                          Camera.main.transform.rotation * Vector3.up);
    }
}
