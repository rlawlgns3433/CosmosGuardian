using UnityEngine;

public class PlanetRotate : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private float radius = 20f;
    private float angle = 0f;
    public float rotationSpeed; // 회전 속도 (도/초)
    public Quaternion tilt; // 기울기

    void Start()
    {
        if (!GameObject.FindWithTag(Tags.Player).TryGetComponent(out playerMovement))
        {
            playerMovement.enabled = false;
            return;
        }
        rotationSpeed = Random.Range(-10,10);
        radius = Random.Range(30, 100);

        // 기울기 설정 (예: 30도 기울기)

        int randomX = Random.Range(-20, 20);
        int randomZ = Random.Range(-20, 20);

        tilt = Quaternion.Euler(randomX, 0, randomZ);

        transform.position = Random.onUnitSphere * radius;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerMovement == null)
        {
            return;
        }

        // 각도를 증가시킴
        angle += rotationSpeed * Time.deltaTime;
        if (angle > 360f)
        {
            angle -= 360f;
        }

        // 새로운 위치 계산
        Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));
        Vector3 tiltedDirection = tilt * direction; // 기울어진 방향으로 변환
        Vector3 newPosition = playerMovement.transform.position + tiltedDirection * radius;

        transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z);
    }
}
