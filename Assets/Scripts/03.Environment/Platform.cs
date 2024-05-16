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

    [Tooltip("리셋 횟수")]
    public int resetCount = 0;
    [Tooltip("배율")]
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
        Spawn(); // 몬스터 스폰
        optionController.ResetOptions(playerStats.level); // 옵션 초기화
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

                    // EnemyData 객체를 복사하여 사용
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
                        Debug.LogError("EnemyData를 찾을 수 없습니다.");
                    }
                }
            }
        }
    }




    // 랜덤한 Row 타일의 하위에 있는 Box 콜라이더들을 가져온다.
    // 그 Box 콜라이더의 랜덤 위치에 몬스터를 생성한다.
    // 하나의 Box 콜라이더에 몬스터는 최소 1마리 최대 3마리까지 배치한다. 

    // 한 번 생성할 때 호출하는 함수
    Vector3 GetRandomPositionOnObject(BoxCollider boxCollider, GameObject tile)
    {
        // BoxCollider의 중심과 크기 가져오기
        Vector3 center = boxCollider.center;
        Vector3 size = boxCollider.size;

        // 게임 오브젝트의 로컬 스케일 적용
        Vector3 scaledSize = Vector3.Scale(size, tile.transform.localScale);

        // BoxCollider의 경계 내에서 임의의 위치 계산
        float randomX = Random.Range(center.x - scaledSize.x / 2, center.x + scaledSize.x / 2);
        float randomY = Random.Range(center.y - scaledSize.y / 2, center.y + scaledSize.y / 2);
        float randomZ = Random.Range(center.z - scaledSize.z / 2, center.z + scaledSize.z / 2);

        // 로컬 좌표를 월드 좌표로 변환
        Vector3 randomLocalPosition = new Vector3(randomX, randomY, randomZ);
        Vector3 randomWorldPosition = tile.transform.TransformPoint(randomLocalPosition);

        return randomWorldPosition;
    }
}
