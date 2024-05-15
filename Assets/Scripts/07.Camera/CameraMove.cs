using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    private Vector3 offset = Vector3.zero;
    private float speed = 15f;
    private Vector3 velocity = Vector3.zero;  // SmoothDamp 사용을 위한 속도 벡터

    private void Start()
    {
        offset = target.position - transform.position;
    }

    private void LateUpdate()
    {
        Vector3 camPos = transform.position;
        camPos.x = 0;

        transform.position = Vector3.SmoothDamp(transform.position, target.position - offset, ref velocity, speed * Time.deltaTime);
    }
}
