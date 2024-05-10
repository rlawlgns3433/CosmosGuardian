using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerInput playerInput = null;
    public DynamicJoystick dynamicJoystick;
    private Vector3 forward = Vector3.forward;
    private new Rigidbody rigidbody;
    private float speedVertical = 5.0f; // �ӽ� ���� �ӷ�
    private float speedHorizontal = 2.0f; // �ӽ� ���� �ӷ�

    private void Start()
    {
        if(!TryGetComponent(out rigidbody))
        {
            return;
        }
    }

    private void Update()
    {
        // �÷��̾� X�� �̵��� Axis�� PlayerStats�� Horizontal Speed�� ������ ���
        // ���� �ӽ� �ӷ� ���
#if UNITY_ANDROID
        rigidbody.MovePosition(transform.position + (forward * speedVertical + Vector3.right * dynamicJoystick.Horizontal * speedHorizontal) * Time.deltaTime);
#elif UNITY_EDITOR_WIN
        rigidbody.MovePosition(transform.position + (forward * speedVertical + Vector3.right * playerInput.moveX * speedHorizontal) * Time.deltaTime);

#endif
    }





    //private Vector3 forward = Vector3.forward;
    //private float speedVertical = 5.0f; // �ӽ� ���� �ӷ�
    //private float speedHorizontal = 2.0f; // �ӽ� ���� �ӷ�
    //public DynamicJoystick dynamicJoystick;
    //public Rigidbody rb;

    //public void FixedUpdate()
    //{
    //    //Vector3 direction = Vector3.forward * dynamicJoystick.Vertical + Vector3.right * dynamicJoystick.Horizontal;
    //    //rb.MovePosition(transform.position + direction * speed * Time.fixedDeltaTime);

    //    rb.MovePosition(transform.position + (forward * speedVertical + Vector3.right * dynamicJoystick.Horizontal * speedHorizontal) * Time.deltaTime);

    //}
}
