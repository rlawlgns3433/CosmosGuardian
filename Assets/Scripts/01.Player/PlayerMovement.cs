using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerInput playerInput = null;
    public PlayerStats playerStats = null;
    public DynamicJoystick dynamicJoystick;
    private Vector3 forward = Vector3.forward;
    public Rigidbody rb;
    private float verticalCoefficient = 5.0f; // 수직 속력 계수
    private float horizontalCoefficient = 2.0f; // 수평 속력 계수

    private void Update()
    {
        // 플레이어 X축 이동은 Axis와 PlayerStats의 Horizontal Speed의 곱으로 사용
        // 현재 임시 속력 사용
#if UNITY_ANDROID
        rb.MovePosition(transform.position 
            + (forward * playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_V] * verticalCoefficient + Vector3.right * dynamicJoystick.Horizontal * playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_H] * horizontalCoefficient) * Time.deltaTime);
#elif UNITY_EDITOR_WIN
        rb.MovePosition(transform.position 
        + (forward * playerStats.speedVertical * verticalCoefficient + Vector3.right * dynamicJoystick.Horizontal * playerStats.speedHorizontal * horizontalCoefficient) * Time.deltaTime);
#endif
    }
}
