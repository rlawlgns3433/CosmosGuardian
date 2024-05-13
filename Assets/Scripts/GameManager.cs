using JetBrains.Annotations;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public EnemySpawnController enemySpawnController;
    private Coroutine enemySpawnCoroutine;

    // �÷��� ���� �ʿ�
    public List<GameObject> platforms = new List<GameObject>();

    public int currentPlatformIndex = 0;

    public PlayerStats playerStats = null;
    public bool IsGameover { get; set; }

    public float platformSpacing = 27f;

    private void Update()
    {
        GameObject currentPlatform = platforms[currentPlatformIndex];
        if (currentPlatform.transform.position.z + platformSpacing < playerStats.gameObject.transform.position.z)
        {
            enemySpawnCoroutine = StartCoroutine(enemySpawnController.SpawnEnemy());
            Vector3 lastPlatformPosition = platforms[platforms.Count - 1].transform.position;
            Vector3 newPlatformPosition = lastPlatformPosition + new Vector3(0, 0, platformSpacing);
            currentPlatform.transform.position = newPlatformPosition;

            platforms.RemoveAt(currentPlatformIndex);
            platforms.Add(currentPlatform);
        }
    }


    public void Gameover()
    {
        StopCoroutine(enemySpawnCoroutine);

        playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_H] = 0;
        playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_V] = 0;
        playerStats.gameObject.GetComponent<PlayerShooter>().enabled = false;
        playerStats.gameObject.GetComponent<PlayerInput>().enabled = false;

        Collider[] colliders = playerStats.gameObject.GetComponents<Collider>();

        foreach(var collider in colliders)
        {
            collider.enabled = false;
        }

        foreach(var enemy in enemySpawnController.spawnedEnemies)
        {
            enemy.direction = Vector3.zero;
        }
    }
}
