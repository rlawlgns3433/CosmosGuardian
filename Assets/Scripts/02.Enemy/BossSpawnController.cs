using UnityEngine;

public class BossSpawnController : MonoBehaviour
{
    // midboss ���� Ƚ���� �����;� ��
    // n�� midboss�� �����ϸ� �������� boss�� �����ؾ� ��
    public GameObject bossSpawnTile;
    public GameObject bossPrefab;
    public GameObject midBossPrefab;
    public int midBossSpawnCount = 0;
    [Tooltip("midboss�� �� �� ���� �� ���� �����ϴ��� �����ϴ� ����")]
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

        // �߰� ���� ����
        ++midBossSpawnCount;
        var spawnPos = bossSpawnTile.transform.position;
        spawnPos += new Vector3(0, 1f, 0);
        var go = Instantiate(midBossPrefab, spawnPos, Quaternion.Euler(0, 180, 0));
        var midBoss = go.GetComponent<Enemy>();
        EnemyData midBossData = new EnemyData(platform.enemyTable.Get((int)EnemyType.MidBoss));
        midBoss.UpdateStats(midBossData, midBossData.MAGNIFICATION, platform.resetCount);
    }
}
