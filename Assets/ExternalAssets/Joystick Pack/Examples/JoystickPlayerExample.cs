using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickPlayerExample : MonoBehaviour
{
    private Vector3 forward = Vector3.forward;
    private float speedVertical = 5.0f; // 임시 수직 속력
    private float speedHorizontal = 2.0f; // 임시 수평 속력
    public DynamicJoystick dynamicJoystick;
    public Rigidbody rb;

    public void FixedUpdate()
    {
        //Vector3 direction = Vector3.forward * dynamicJoystick.Vertical + Vector3.right * dynamicJoystick.Horizontal;
        //rb.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);

        rb.MovePosition(transform.position + (forward * speedVertical + Vector3.right * dynamicJoystick.Horizontal * speedHorizontal) * Time.deltaTime);

    }
}