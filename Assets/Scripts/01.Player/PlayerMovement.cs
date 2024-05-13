using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerInput playerInput = null;
    public PlayerStats playerStats = null;
    public DynamicJoystick dynamicJoystick;
    private Vector3 forward = Vector3.forward;
    public Rigidbody rb;
    private float verticalCoefficient = 5.0f; // ���� �ӷ� ���
    private float horizontalCoefficient = 2.0f; // ���� �ӷ� ���

    private void Update()
    {
        // �÷��̾� X�� �̵��� Axis�� PlayerStats�� Horizontal Speed�� ������ ���
        // ���� �ӽ� �ӷ� ���
#if UNITY_ANDROID
        rb.MovePosition(transform.position 
            + (forward * playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_V] * verticalCoefficient + Vector3.right * dynamicJoystick.Horizontal * playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_H] * horizontalCoefficient) * Time.deltaTime);
#elif UNITY_EDITOR_WIN
        rb.MovePosition(transform.position 
        + (forward * playerStats.speedVertical * verticalCoefficient + Vector3.right * dynamicJoystick.Horizontal * playerStats.speedHorizontal * horizontalCoefficient) * Time.deltaTime);
#endif
    }
}
