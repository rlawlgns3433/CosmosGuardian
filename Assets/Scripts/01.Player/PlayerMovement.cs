using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private const float verticalCoefficient = 4.5f; // ���� �ӷ� ���
    private const float horizontalCoefficient = 4.5f; // ���� �ӷ� ���
    private const float minX = -4.05f;
    private const float maxX = 4.05f;
    public PlayerStats playerStats = null;
    public FloatingJoystick floatingJoystick;
    public Rigidbody rb;
    private Vector3 forward = Vector3.forward;
    private Vector3 movement;

    private void FixedUpdate()
    {
        movement = forward * playerStats.stats.stat[CharacterColumn.Stat.MOVE_SPEED_V] * verticalCoefficient + 
                   Vector3.right * floatingJoystick.Horizontal * playerStats.stats.stat[CharacterColumn.Stat.MOVE_SPEED_H] * horizontalCoefficient;
        
        movement *= Time.deltaTime;

        Vector3 newPosition = transform.position + movement;
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        rb.MovePosition(newPosition);
    }
}
