using UnityEngine;

public class BossSpawnController : MonoBehaviour
{
    public GameObject bossSpawnTile;
    public GameObject bossPrefab;
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
}
