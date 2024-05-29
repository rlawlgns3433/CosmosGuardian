using UnityEngine;

public class PlanetRotate : MonoBehaviour
{
    public float rotationSpeed; // 회전 속도 (도/초)
    public Quaternion tilt; // 기울기

    private float radius = 20f;
    private float angle = 0f;

    void Start()
    {
        rotationSpeed = Random.Range(-10,10);
        radius = Random.Range(30, 100);

        int randomX = Random.Range(-20, 20);
        int randomZ = Random.Range(-20, 20);

        tilt = Quaternion.Euler(randomX, 0, randomZ);
        transform.position = Random.onUnitSphere * radius;
    }

    void Update()
    {
        angle += rotationSpeed * Time.deltaTime;
        if (angle > 360f)
        {
            angle -= 360f;
        }

        Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));
        Vector3 tiltedDirection = tilt * direction; 
        Vector3 newPosition = Camera.main.transform.position + tiltedDirection * radius;

        transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z);
    }
}
