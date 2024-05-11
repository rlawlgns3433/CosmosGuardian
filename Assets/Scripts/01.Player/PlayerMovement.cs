using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerInput playerInput = null;
    public PlayerStats playerStats = null;
    public DynamicJoystick dynamicJoystick;
    private Vector3 forward = Vector3.forward;
    public Rigidbody rb;
    private float verticalCoefficient = 5.0f; // �ӽ� ���� �ӷ�
    private float horizontalCoefficient = 2.0f; // �ӽ� ���� �ӷ�



    private void Update()
    {
        // �÷��̾� X�� �̵��� Axis�� PlayerStats�� Horizontal Speed�� ������ ���
        // ���� �ӽ� �ӷ� ���
#if UNITY_ANDROID
        rb.MovePosition(transform.position 
            + (forward * playerStats.speedVertical * verticalCoefficient + Vector3.right * dynamicJoystick.Horizontal * playerStats.speedHorizontal * horizontalCoefficient) * Time.deltaTime);
#elif UNITY_EDITOR_WIN
        rb.MovePosition(transform.position 
        + (forward * playerStats.speedVertical * verticalCoefficient + Vector3.right * dynamicJoystick.Horizontal * playerStats.speedHorizontal * horizontalCoefficient) * Time.deltaTime);
#endif
    }
}
