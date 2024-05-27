using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Enemy;

public class Platform : MonoBehaviour
{
    [Tooltip("���� Ƚ��")]
    public int resetCount = 0;
    [Tooltip("����")]
    public float hpScale = 1.8f;
    [Tooltip("�����ӵ� ������")]
    public float verticalIncrement = 0.5f;
    public int minTest = 1;
    public int maxTest = 4;
    public OptionController optionController;
    public List<GameObject> enemySpawnTile = new List<GameObject>();
    public Dictionary<EnemyType, List<Enemy>> spawnedEnemies = new Dictionary<EnemyType, List<Enemy>>();
    public Dictionary<EnemyType, List<Enemy>> unusingEnemies = new Dictionary<EnemyType, List<Enemy>>();
    public EnemyTable enemyTable;

    private PlayerStats playerStats;

    private void Start()
    {
        spawnedEnemies[EnemyType.Normal] = new List<Enemy>();
        spawnedEnemies[EnemyType.Elite] = new List<Enemy>();
        spawnedEnemies[EnemyType.MidBoss] = new List<Enemy>();
        spawnedEnemies[EnemyType.Boss] = new List<Enemy>();

        unusingEnemies[EnemyType.Normal] = new List<Enemy>();
        unusingEnemies[EnemyType.Elite] = new List<Enemy>();
        unusingEnemies[EnemyType.MidBoss] = new List<Enemy>();
        unusingEnemies[EnemyType.Boss] = new List<Enemy>();

        enemyTable = DataTableMgr.Get<EnemyTable>(DataTableIds.Enemy);
        playerStats = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerStats>();
        Spawn();
    }

    public void ResetPlatform()
    {
        foreach (var enemies in spawnedEnemies.Values)
        {
            foreach (var enemy in enemies)
            {
                if(enemy.chaseCoroutine != null)
                {
                    enemy.StopCoroutine(enemy.chaseCoroutine);
                }
                enemy.isAlive = false;
                enemy.isChasing = false;
                enemy.gameObject.SetActive(false);
                unusingEnemies[enemy.enemyType].Add(enemy);

                //if (enemy != null)
                //{
                //    enemy.OnDie();
                //    // return �κ�
                //}
            }
        }

        ++resetCount;
        //spawnedEnemies.Clear();

        if (maxTest != 10)
        {
            if ((resetCount / 2) % 2 == 0)
            {
                ++maxTest;
            }
            else
            {
                ++minTest;
            }
        }

        Spawn(); // ���� ����
        if (TryGetComponent(out BossSpawnController bossSpawnController))
        {
            bossSpawnController.SpawnMidBoss();
        }
        optionController.ResetOptions(playerStats.level); // �ɼ� �ʱ�ȭ
        playerStats.UpdateStats(OptionColumn.Stat.MOVE_SPEED_V, OptionColumn.Type.Scale, verticalIncrement); // ���� �ӵ� 0.5���� ����
    }

    public void Spawn()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager �ν��Ͻ��� null�Դϴ�. ���� ������ �� �����ϴ�.");
            return;
        }

        foreach (var row in enemySpawnTile)
        {
            BoxCollider[] colliders = row.GetComponentsInChildren<BoxCollider>();
            foreach (var collider in colliders)
            {
                int spawnCount = Random.Range(minTest, maxTest);

                for (int i = 0; i < spawnCount; ++i)
                {
                    Vector3 spawnPos = GetRandomPositionOnObject(collider, collider.gameObject);
                    spawnPos.y = 1;
                    int rand = Random.Range(0, GameManager.Instance.enemies.Count);


                    Enemy enemy;
                    var selectedEnemyType = GameManager.Instance.enemies[rand].GetComponent<Enemy>().enemyType;
                    // ���� ��
                    if (unusingEnemies[selectedEnemyType].Count <= 0)
                    {
                        var go = Instantiate(GameManager.Instance.enemies[rand], spawnPos, Quaternion.Euler(new Vector3(0, 180, 0)));
                        enemy = go.GetComponent<Enemy>();
                        spawnedEnemies[enemy.enemyType].Add(enemy);
                    }
                    // ���� ��
                    else
                    {
                        enemy = GetEnemy(selectedEnemyType);
                        enemy.gameObject.transform.position = spawnPos;
                        enemy.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    }

                    // EnemyData ��ü�� �����Ͽ� ���
                    EnemyData originalData = enemyTable.Get((int)enemy.enemyType);
                    if (originalData != null)
                    {
                        EnemyData dataCopy = new EnemyData(originalData);
                        enemy.UpdateStats(dataCopy, dataCopy.MAGNIFICATION, resetCount);
                    }
                    else
                    {
                        Debug.LogError("EnemyData�� ã�� �� �����ϴ�.");
                    }
                }
            }
        }
    }

    Vector3 GetRandomPositionOnObject(BoxCollider boxCollider, GameObject tile)
    {
        Vector3 center = boxCollider.center;
        Vector3 size = boxCollider.size;

        Vector3 scaledSize = Vector3.Scale(size, tile.transform.localScale);

        float randomX = Random.Range(center.x - scaledSize.x / 2, center.x + scaledSize.x / 2);
        float randomY = Random.Range(center.y - scaledSize.y / 2, center.y + scaledSize.y / 2);
        float randomZ = Random.Range(center.z - scaledSize.z / 2, center.z + scaledSize.z / 2);

        Vector3 randomLocalPosition = new Vector3(randomX, randomY, randomZ);
        Vector3 randomWorldPosition = tile.transform.TransformPoint(randomLocalPosition);

        return randomWorldPosition;
    }

    IEnumerator DelaySpawn(float t)
    {
        yield return new WaitForSeconds(t);
        Spawn();
    }

    public Enemy GetEnemy(EnemyType enemyType)
    {
        if (unusingEnemies[enemyType].Count <= 0)
            return null;

        var enemy = unusingEnemies[enemyType][0];
        enemy.gameObject.SetActive(true);
        enemy.StopAllCoroutines();

        enemy.isAlive = true;
        enemy.enemyState = EnemyState.Idle;

        enemy.textHealth.gameObject.SetActive(true);
        enemy.sphereCollider.enabled = true;

        enemy.speed = enemy.originSpeed;

        unusingEnemies[enemyType].Remove(enemy);
        spawnedEnemies[enemyType].Add(enemy);

        Debug.Log(enemy.name);

        return enemy;
    }
}
