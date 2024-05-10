using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerInput playerInput = null;
    private Vector3 forward = Vector3.forward;
    private new Rigidbody rigidbody;
    private float speedVertical = 5.0f; // 임시 수직 속력
    private float speedHorizontal = 2.0f; // 임시 수평 속력

    private void Start()
    {
        if(!TryGetComponent(out rigidbody))
        {
            return;
        }
    }

    private void Update()
    {
        // 플레이어 X축 이동은 Axis와 PlayerStats의 Horizontal Speed의 곱으로 사용
        // 현재 임시 속력 사용

        rigidbody.MovePosition(transform.position + (forward * speedVertical + Vector3.right * playerInput.moveX * speedHorizontal) * Time.deltaTime);
    }
}
