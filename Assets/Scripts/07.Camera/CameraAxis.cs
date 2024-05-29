using UnityEngine;

public class CameraAxis : MonoBehaviour
{
    public Transform player;
    private CameraMove cameraMove;

    private void Start()
    {
        if(!Camera.main.TryGetComponent(out cameraMove))
        {
            cameraMove.enabled = false;
            return;
        }
    }

    private void Update()
    {
        Vector3 pos = player.position;
        pos.x = cameraMove.IsTOP ? 0f : pos.x;
        transform.position = pos;
    }
}