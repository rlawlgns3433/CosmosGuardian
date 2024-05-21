using UnityEngine;

public class BossSpawnController : MonoBehaviour
{
    // midboss 등장 횟수를 가져와야 함
    // n번 midboss가 등장하면 다음에는 boss가 등장해야 함
    public GameObject bossSpawnTile;
    public GameObject bossPrefab;
    public GameObject midBossPrefab;
    public int midBossSpawnCount = 0;
    [Tooltip("midboss가 몇 번 등장 후 보스 등장하는지 관리하는 변수")]
    public int bossAppearanceThreshold = 3;
    private Platform platform;

    private void Start()
    {
        if(!TryGetComponent(out platform))
        {
            platform.enabled = false;
            return;
        }
    }

    public void SpawnBoss()
    {
        // 현재 플랫폼에 있는 몬스터 전체 삭제
        foreach (var enemy in platform.spawnedEnemies)
        {
            if (enemy != null)
            {
                enemy.OnDie();
            }
        }

        var spawnPos = bossSpawnTile.transform.position;
        spawnPos += new Vector3(0, 1f, 0);

        var go = Instantiate(bossPrefab, spawnPos, Quaternion.Euler(0, 180, 0));
        var boss = go.GetComponent<Boss>();

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

        // 중간 보스 등장
        ++midBossSpawnCount;
        var spawnPos = bossSpawnTile.transform.position;
        spawnPos += new Vector3(0, 1f, 0);

        var go = Instantiate(midBossPrefab, spawnPos, Quaternion.Euler(0, 180, 0));
        var midBoss = go.GetComponent<MidBoss>();

        EnemyData midBossData = new EnemyData(platform.enemyTable.Get((int)EnemyType.MidBoss));
        midBoss.UpdateStats(midBossData, midBossData.MAGNIFICATION, platform.resetCount);
    }
}
