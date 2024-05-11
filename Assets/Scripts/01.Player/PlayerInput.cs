using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public static readonly string horizontal = "Horizontal";

    public float moveX { get; set; }

    private void Update()
    {
        moveX = Input.GetAxis(horizontal);
    }
}
