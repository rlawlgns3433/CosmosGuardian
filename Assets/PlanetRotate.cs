using UnityEngine;

public class PlanetRotate : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private float radius = 20f;
    private float angle = 0f;
    public float rotationSpeed; // ȸ�� �ӵ� (��/��)
    public Quaternion tilt; // ����

    void Start()
    {
        if (!GameObject.FindWithTag(Tags.Player).TryGetComponent(out playerMovement))
        {
            playerMovement.enabled = false;
            return;
        }
        rotationSpeed = Random.Range(-10,10);
        radius = Random.Range(30, 100);

        // ���� ���� (��: 30�� ����)

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

        // ������ ������Ŵ
        angle += rotationSpeed * Time.deltaTime;
        if (angle > 360f)
        {
            angle -= 360f;
        }

        // ���ο� ��ġ ���
        Vector3 direction = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), 0, Mathf.Sin(angle * Mathf.Deg2Rad));
        Vector3 tiltedDirection = tilt * direction; // ������ �������� ��ȯ
        Vector3 newPosition = playerMovement.transform.position + tiltedDirection * radius;

        transform.position = new Vector3(newPosition.x, newPosition.y, newPosition.z);
    }
}
