using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAxis : MonoBehaviour
{
    public Transform player;

    private void Update()
    {
        Vector3 pos = player.position;
        pos.x = 0;
        transform.position = pos;
    }
}
