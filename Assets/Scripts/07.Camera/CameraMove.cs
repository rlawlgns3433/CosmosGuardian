using UnityEngine;

public class CameraMove : MonoBehaviour
{
    //public Transform target;
    //private Vector3 offset = Vector3.zero;

    //private void Start()
    //{
    //    offset = target.position - transform.position;
    //}

    //private void LateUpdate()
    //{
    //    Vector3 camPos = target.position -offset;
    //    camPos.x = 0;

    //    transform.position = camPos;
    //}

    public Transform TPS, TOP;
    public bool IsTOP = true;

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Y))
        {
            IsTOP = !IsTOP;
        }
        if (IsTOP)
            Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, TOP.position, 5 * Time.deltaTime);
        else
            Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, TPS.position, 5 * Time.deltaTime);

    }
}
