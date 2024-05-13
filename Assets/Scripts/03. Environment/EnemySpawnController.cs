using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour
{
    public List<Enemy> enemies = new List<Enemy>(); // GameManager에서 갖는 것이 좋겠음.
    public List<Enemy> spawnedEnemies = new List<Enemy>();

    private readonly WaitForSeconds spawnInterval = new WaitForSeconds(0.5f);
    private readonly int minSpawnCount = 5;
    private readonly int maxSpawnCount = 20;
    private int spawnCount = 0;
    private int currentSpawnedCount = 0;
    private int nextSpawnpoint = 1;

    public IEnumerator SpawnEnemy()
    {
        spawnCount = UnityEngine.Random.Range(minSpawnCount, maxSpawnCount);
        currentSpawnedCount = 0;
        var spawnPos = GameManager.Instance.platforms[nextSpawnpoint].GetComponent<Platform>().enemySpawnTile[0].transform.position;
        spawnPos += new Vector3(0, 0.5f, 0);
        while (currentSpawnedCount < spawnCount)
        {
            int rand = UnityEngine.Random.Range(0, enemies.Count);
            var enemy = Instantiate(enemies[rand], spawnPos, Quaternion.identity);
            spawnedEnemies.Add(enemy);
            currentSpawnedCount++;
            yield return spawnInterval;
        }
    }
}
