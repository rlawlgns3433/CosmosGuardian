using System.Collections;
using System.Collections.Generic;
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
    }

}
