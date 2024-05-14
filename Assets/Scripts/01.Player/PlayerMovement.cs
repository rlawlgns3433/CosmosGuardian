using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public PlayerInput playerInput = null;
    public PlayerStats playerStats = null;
    public DynamicJoystick dynamicJoystick;
    private Vector3 forward = Vector3.forward;
    public Rigidbody rb;
    private float verticalCoefficient = 4.5f; // ���� �ӷ� ���
    private float horizontalCoefficient = 3.0f; // ���� �ӷ� ���
    private float minX = -4.05f;
    private float maxX = 4.05f;


    //    private void Update()
    //    {
    //        // �÷��̾� X�� �̵��� Axis�� PlayerStats�� Horizontal Speed�� ������ ���
    //        // ���� �ӽ� �ӷ� ���
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
        // �̵� �Ÿ� ���
        Vector3 movement;
#if UNITY_ANDROID
        movement = forward * playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_V] * verticalCoefficient + Vector3.right * dynamicJoystick.Horizontal * playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_H] * horizontalCoefficient;
#elif UNITY_EDITOR_WIN
        movement = forward * playerStats.speedVertical * verticalCoefficient + Vector3.right * dynamicJoystick.Horizontal * playerStats.speedHorizontal * horizontalCoefficient;
#endif
        movement *= Time.deltaTime;

        // �� ��ġ ���
        Vector3 newPosition = transform.position + movement;

        // X ��ġ ����
        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        // Rigidbody �̵�
        rb.MovePosition(newPosition);
    }

}
