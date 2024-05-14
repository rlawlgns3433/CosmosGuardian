using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerInput playerInput = null;
    public PlayerStats playerStats = null;
    public DynamicJoystick dynamicJoystick;
    private Vector3 forward = Vector3.forward;
    public Rigidbody rb;
    private float verticalCoefficient = 4.5f; // 수직 속력 계수
    private float horizontalCoefficient = 3.0f; // 수평 속력 계수
    private float minX = -4.05f;
    private float maxX = 4.05f;


    //    private void Update()
    //    {
    //        // 플레이어 X축 이동은 Axis와 PlayerStats의 Horizontal Speed의 곱으로 사용
    //        // 현재 임시 속력 사용
    //#if UNITY_ANDROID
    //        rb.MovePosition(transform.position 
    //            + (forward * playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_V] * verticalCoefficient + Vector3.right * dynamicJoystick.Horizontal * playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_H] * horizontalCoefficient) * Time.deltaTime);
    //#elif UNITY_EDITOR_WIN
    //        rb.MovePosition(transform.position 
    //        + (forward * playerStats.speedVertical * verticalCoefficient + Vector3.right * dynamicJoystick.Horizontal * playerStats.speedHorizontal * horizontalCoefficient) * Time.deltaTime);
    //#endif
    //        //if(transform.position.x < minX)
    //        //{
    //        //    transform.position = new Vector3(minX, transform.position.y, transform.position.z);
    //        //}

    //        //if (transform.position.x > maxX)
    //        //{
    //        //    transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
    //        //}

    //        if (transform.position.x < minX)
    //        {
    //            Vector3 newPosition = transform.position;
    //            newPosition.x = minX;
    //            transform.position = newPosition;
    //        }

    //        if (transform.position.x > maxX)
    //        {
    //            Vector3 newPosition = transform.position;
    //            newPosition.x = maxX;
    //            transform.position = newPosition;
    //        }
    //    }

    private void FixedUpdate()
    {
        // 이동 거리 계산
        Vector3 movement;
#if UNITY_ANDROID
        movement = forward * playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_V] * verticalCoefficient + Vector3.right * dynamicJoystick.Horizontal * playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_H] * horizontalCoefficient;
#elif UNITY_EDITOR_WIN
        movement = forward * playerStats.speedVertical * verticalCoefficient + Vector3.right * dynamicJoystick.Horizontal * playerStats.speedHorizontal * horizontalCoefficient;
#endif
        movement *= Time.deltaTime;

        // 새 위치 계산
        Vector3 newPosition = transform.position + movement;

        // X 위치 제한
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        // Rigidbody 이동
        rb.MovePosition(newPosition);
    }

}
