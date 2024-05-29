using UnityEngine;

public class BossSpawnController : MonoBehaviour
{
    public Platform platform;
    public GameObject bossSpawnTile;
    public GameObject bossPrefab;
    public GameObject midBossPrefab;
    [Tooltip("midboss�� �� �� ���� �� ���� �����ϴ��� �����ϴ� ����")]
    public int bossAppearanceThreshold = 3;
    public int midBossSpawnCount = -1;

    private void Start()
    {
        SpawnMidBoss();
    }

    public void SpawnBoss()
    {
        foreach (var enemies in platform.spawnedEnemies.Values)
        {
            foreach (var enemy in enemies)
            {
                platform.ReturnEnemy(enemy);
            }
        }

        var spawnPos = bossSpawnTile.transform.position;
        spawnPos += new Vector3(0, 1f, 0);

        var go = Instantiate(bossPrefab, spawnPos, Quaternion.Euler(0, 180, 0));
        var boss = go.GetComponent<Boss>();
        platform.spawnedEnemies[boss.enemyType].Add(boss);

        EnemyData bossData = new EnemyData(platform.enemyTable.Get((int)EnemyType.Boss));
        boss.UpdateStats(bossData, bossData.MAGNIFICATION, platform.resetCount);
    }

    public void SpawnMidBoss()
    {
        if (midBossSpawnCount >= bossAppearanceThreshold)
        {
            SpawnBoss();
            midBossSpawnCount = 0;
            return;
        }

        ++midBossSpawnCount;
        var spawnPos = bossSpawnTile.transform.position;
        spawnPos += new Vector3(0, 1f, 0);

        var go = Instantiate(midBossPrefab, spawnPos, Quaternion.Euler(0, 180, 0));
        var midBoss = go.GetComponent<MidBoss>();
        platform.spawnedEnemies[midBoss.enemyType].Add(midBoss);

        EnemyData midBossData = new EnemyData(platform.enemyTable.Get((int)EnemyType.MidBoss));
        midBoss.UpdateStats(midBossData, midBossData.MAGNIFICATION, platform.resetCount);
    }
}