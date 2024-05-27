using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerInput playerInput = null;
    public PlayerStats playerStats = null;
    public FloatingJoystick floatingJoystick;
    private Vector3 forward = Vector3.forward;
    public Rigidbody rb;
    private float verticalCoefficient = 4.5f; // 수직 속력 계수
    private float horizontalCoefficient = 4.5f; // 수평 속력 계수
    private float minX = -4.05f;
    private float maxX = 4.05f;
    Vector3 movement;

    private void FixedUpdate()
    {
#if UNITY_ANDROID
        movement = forward * playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_V] * verticalCoefficient + Vector3.right * floatingJoystick.Horizontal * playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_H] * horizontalCoefficient;
#elif UNITY_EDITOR
        movement = forward * playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_V]* verticalCoefficient + Vector3.right * floatingJoystick.Horizontal * playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_H] * horizontalCoefficient;
#elif PLATFORM_STANDALONE_WIN
        movement = forward * playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_V]* verticalCoefficient + Vector3.right * floatingJoystick.Horizontal * playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_H] * horizontalCoefficient;
#endif
        movement *= Time.deltaTime;

        Vector3 newPosition = transform.position + movement;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        rb.MovePosition(newPosition);
    }
}
