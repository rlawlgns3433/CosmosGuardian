using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public bool IsTOP = true;
    public Transform TPS, TOP;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            IsTOP = !IsTOP;
        }
        if (IsTOP)
        {
            Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, TOP.position, 5 * Time.deltaTime);
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, TOP.rotation, 5 * Time.deltaTime);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 80f, 5 * Time.deltaTime);
        }
        else
        {
            Camera.main.transform.position = Vector3.Slerp(Camera.main.transform.position, TPS.position, 5 * Time.deltaTime);
            Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, TPS.rotation, 5 * Time.deltaTime);
            Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, 120f, 5 * Time.deltaTime);
        }
    }
}
