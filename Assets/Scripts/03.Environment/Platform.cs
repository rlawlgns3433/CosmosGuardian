using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public int minTest = 1;
    public int maxTest = 4;
    private PlayerStats playerStats;
    public OptionController optionController;
    public List<GameObject> enemySpawnTile = new List<GameObject>();
    public List<Enemy> spawnedEnemies = new List<Enemy>();
    public EnemyTable enemyTable;

    [Tooltip("���� Ƚ��")]
    public int resetCount = 0;
    [Tooltip("����")]
    public float hpScale = 1.8f;

    private void OnEnable()
    {
        enemyTable = DataTableMgr.Get<EnemyTable>(DataTableIds.Enemy);
        playerStats = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerStats>();
        Spawn();
    }

    public void ResetPlatform()
    {
        foreach (var enemy in spawnedEnemies)
        {
            if (enemy != null)
            {
                enemy.OnDie();
            }
        }

        ++resetCount;
        spawnedEnemies.Clear();
        Spawn(); // ���� ����
        optionController.ResetOptions(playerStats.level); // �ɼ� �ʱ�ȭ
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
                    int rand = Random.Range(0, GameManager.Instance.enemies.Count);
                    var enemy = Instantiate(GameManager.Instance.enemies[rand], spawnPos, Quaternion.identity);

                    // EnemyData ��ü�� �����Ͽ� ���
                    EnemyData originalData = enemyTable.Get((int)enemy.enemyType);
                    if (originalData != null)
                    {
                        EnemyData dataCopy = new EnemyData(originalData);
                        enemy.UpdateStats(dataCopy, hpScale, resetCount);

                        Debug.Log($"{enemy.enemyType} : {enemy.enemyData.HP}");
                        spawnedEnemies.Add(enemy);
                    }
                    else
                    {
                        Debug.LogError("EnemyData�� ã�� �� �����ϴ�.");
                    }
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
