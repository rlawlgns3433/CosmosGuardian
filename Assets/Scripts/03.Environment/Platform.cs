using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Platform : MonoBehaviour
{
    [Tooltip("리셋 횟수")]
    public int resetCount = 0;
    [Tooltip("배율")]
    public float hpScale = 1.8f;
    [Tooltip("직진속도 증가량")]
    public float verticalIncrement = 0.5f;
    public int minEnemy = 1;
    public int maxEnemy = 4;
    public bool isOn = false;
    public OptionController optionController;
    public List<GameObject> enemySpawnTile = new List<GameObject>();
    public Dictionary<EnemyType, List<Enemy>> spawnedEnemies = new Dictionary<EnemyType, List<Enemy>>();
    public Dictionary<EnemyType, List<Enemy>> unusingEnemies = new Dictionary<EnemyType, List<Enemy>>();
    public EnemyTable enemyTable;
    private BossSpawnController bossSpawnController;
    private PlayerStats playerStats;

    private void Awake()
    {
        spawnedEnemies[EnemyType.Bat] = new List<Enemy>();
        spawnedEnemies[EnemyType.Dragon] = new List<Enemy>();
        spawnedEnemies[EnemyType.Elite] = new List<Enemy>();
        spawnedEnemies[EnemyType.MidBoss] = new List<Enemy>();
        spawnedEnemies[EnemyType.Boss] = new List<Enemy>();

        unusingEnemies[EnemyType.Bat] = new List<Enemy>();
        unusingEnemies[EnemyType.Dragon] = new List<Enemy>();
        unusingEnemies[EnemyType.Elite] = new List<Enemy>();
        unusingEnemies[EnemyType.MidBoss] = new List<Enemy>();
        unusingEnemies[EnemyType.Boss] = new List<Enemy>();
    }

    private void Start()
    {
        enemyTable = DataTableMgr.Get<EnemyTable>(DataTableIds.Enemy);
        playerStats = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerStats>();
        Spawn();
    }

    private void Update()
    {
        if (isOn) return;
        if (spawnedEnemies[EnemyType.Boss].Count > 0) return;

        if (Vector3.Distance(Camera.main.transform.position, transform.position) < 120)
        {
            foreach (var enemies in spawnedEnemies.Values)
            {
                foreach (var enemy in enemies)
                {
                    enemy.gameObject.SetActive(true);
                    enemy.textHealth.gameObject.SetActive(true);
                }
            }
            isOn = true;
        }
    }

    public void ResetPlatform()
    {
        foreach (var enemies in spawnedEnemies.Values)
        {
            foreach (var enemy in enemies)
            {
                ReturnEnemy(enemy);
            }
            enemies.Clear();
        }

        ++resetCount;

        if (maxEnemy < 5)
        {

            if(resetCount % 2 == 0)
            {
                if(resetCount % 4 == 0)
                {
                    ++minEnemy;
                }
                else
                {
                    ++maxEnemy;
                }
            }
        }

        Spawn();

        foreach (var enemies in spawnedEnemies.Values)
        {
            foreach (var enemy in enemies)
            {
                enemy.gameObject.SetActive(false);
            }
        }

        if (TryGetComponent(out bossSpawnController))
        {
            bossSpawnController.SpawnMidBoss();
        }
        optionController.ResetOptions(playerStats.level);
        playerStats.UpdateStats(OptionColumn.Stat.MOVE_SPEED_V, OptionColumn.Type.Scale, verticalIncrement);
        if (playerStats.stats.stat[CharacterColumn.Stat.MOVE_SPEED_V] >= 2)
        {
            GPGSMgr.ReportAchievement(MyGPGSIds.crazySpeedAchievement);
        }

        isOn = false;
    }

    public void Spawn()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager 인스턴스가 null입니다. 적을 생성할 수 없습니다.");
            return;
        }

        foreach (var row in enemySpawnTile)
        {
            BoxCollider[] colliders = row.GetComponentsInChildren<BoxCollider>();
            foreach (var collider in colliders)
            {
                int spawnCount = Random.Range(minEnemy, maxEnemy);

                for (int i = 0; i < spawnCount; ++i)
                {
                    Vector3 spawnPos = GetRandomPositionOnObject(collider, collider.gameObject);
                    spawnPos.y = 1;
                    int rand = Random.Range(0, GameManager.Instance.enemies.Count);


                    Enemy enemy;
                    var selectedEnemyType = GameManager.Instance.enemies[rand].GetComponent<Enemy>().enemyType;

                    if (unusingEnemies[selectedEnemyType].Count <= 0)
                    {
                        var go = Instantiate(GameManager.Instance.enemies[rand], spawnPos, Quaternion.Euler(new Vector3(0, 180, 0)));
                        enemy = go.GetComponent<Enemy>();
                        spawnedEnemies[enemy.enemyType].Add(enemy);
                    }
                    else
                    {
                        enemy = GetEnemy(selectedEnemyType);
                        enemy.textHealth.gameObject.SetActive(true);
                        enemy.gameObject.transform.position = spawnPos;
                        enemy.damageFloatingPosition = enemy.textHealth.transform.position;
                        enemy.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                    }

                    EnemyData originalData = enemyTable.Get((int)enemy.enemyType);
                    enemy.UpdateStats(originalData, originalData.MAGNIFICATION, resetCount);
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

    public Enemy GetEnemy(EnemyType enemyType)
    {
        if (unusingEnemies[enemyType].Count <= 0)
            return null;

        var enemy = unusingEnemies[enemyType][0];

        enemy.gameObject.SetActive(true);
        enemy.isAlive = true;
        enemy.enemyState = Enemy.EnemyState.Idle;
        enemy.sphereCollider.enabled = true;
        enemy.speed = enemy.originSpeed;

        unusingEnemies[enemyType].Remove(enemy);
        spawnedEnemies[enemyType].Add(enemy);

        return enemy;
    }

    public void ReturnEnemy(Enemy enemy)
    {
        unusingEnemies[enemy.enemyType].Add(enemy);
        enemy.gameObject.SetActive(false);
    }
}