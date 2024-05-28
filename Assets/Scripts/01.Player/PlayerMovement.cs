using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerInput playerInput = null;
    public PlayerStats playerStats = null;
    public FloatingJoystick floatingJoystick;
    private Vector3 forward = Vector3.forward;
    public Rigidbody rb;
    private const float verticalCoefficient = 4.5f; // 수직 속력 계수
    private const float horizontalCoefficient = 4.5f; // 수평 속력 계수
    private const float minX = -4.05f;
    private const float maxX = 4.05f;
    private Vector3 movement;

    private void FixedUpdate()
    {
        movement = forward * playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_V] * verticalCoefficient + 
                   Vector3.right * floatingJoystick.Horizontal * playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_H] * horizontalCoefficient;
        
        movement *= Time.deltaTime;

        Vector3 newPosition = transform.position + movement;
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        rb.MovePosition(newPosition);
    }
}
