using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private readonly int minTest = 1;
    private readonly int maxTest = 4;
    public List<GameObject> enemySpawnTile = new List<GameObject>();
    public List<Enemy> spawnedEnemies = new List<Enemy>();

    private void OnEnable()
    {
        Spawn();
    }

    public void ResetPlatform()
    {
        // �� ������ �ʿ�
        Spawn();
    }

    public void Spawn()
    {
        foreach (var row in enemySpawnTile)
        {
            BoxCollider[] colliders = row.GetComponentsInChildren<BoxCollider>();
            foreach (var collider in colliders)
            {
                int spawnCount = Random.Range(minTest, maxTest);

                for (int i = 0; i < spawnCount; ++i)
                {
                    Vector3 spawnPos = GetRandomPositionOnObject(collider, collider.gameObject);
                    int rand = Random.Range(0, EnemySpawnController.Instance.enemies.Count);
                    var enemy = Instantiate(EnemySpawnController.Instance.enemies[rand], spawnPos, Quaternion.identity);

                    spawnedEnemies.Add(enemy);
                }
            }
        }
    }


    // ������ Row Ÿ���� ������ �ִ� Box �ݶ��̴����� �����´�.
    // �� Box �ݶ��̴��� ���� ��ġ�� ���͸� �����Ѵ�.
    // �ϳ��� Box �ݶ��̴��� ���ʹ� �ּ� 1���� �ִ� 3�������� ��ġ�Ѵ�. 

    // �� �� ������ �� ȣ���ϴ� �Լ�
    Vector3 GetRandomPositionOnObject(BoxCollider boxCollider, GameObject tile)
    {
        // BoxCollider�� �߽ɰ� ũ�� ��������
        Vector3 center = boxCollider.center;
        Vector3 size = boxCollider.size;

        // ���� ������Ʈ�� ���� ������ ����
        Vector3 scaledSize = Vector3.Scale(size, tile.transform.localScale);

        // BoxCollider�� ��� ������ ������ ��ġ ���
        float randomX = Random.Range(center.x - scaledSize.x / 2, center.x + scaledSize.x / 2);
        float randomY = Random.Range(center.y - scaledSize.y / 2, center.y + scaledSize.y / 2);
        float randomZ = Random.Range(center.z - scaledSize.z / 2, center.z + scaledSize.z / 2);

        // ���� ��ǥ�� ���� ��ǥ�� ��ȯ
        Vector3 randomLocalPosition = new Vector3(randomX, randomY, randomZ);
        Vector3 randomWorldPosition = tile.transform.TransformPoint(randomLocalPosition);

        return randomWorldPosition;
    }
}
