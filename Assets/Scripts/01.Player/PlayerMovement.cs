using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerInput playerInput = null;
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

        rigidbody.MovePosition(transform.position + (forward * speedVertical + Vector3.right * playerInput.moveX * speedHorizontal) * Time.deltaTime);
    }
}
