using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerShooter : MonoBehaviour
{
    public List<GameObject> usingProjectiles = new List<GameObject>();
    public List<GameObject> unusingProjectiles = new List<GameObject>();
    public List<GameObject> projectilePrefabs = new List<GameObject>();
    public GameObject muzzle;
    public PlayerStats playerStats;
    public int currentProjectileIndex = 0;
    private float maxFireRateLevel = 40;
    private float minFireRate = 0.5f;
    public float currentFireRateLevel;
    private float FireRate
    {
        get
        {
            return minFireRate - (0.4f * currentFireRateLevel) / maxFireRateLevel;
        }
    }
    public float speed = 20f;
    private float lastFireTime = 0f;
    private Vector3 direction = Vector3.forward;

    private void Start()
    {
        Debug.Log(playerStats.stats[CharacterColumn.Stat.FIRE_RATE]);

        currentFireRateLevel = playerStats.stats[CharacterColumn.Stat.FIRE_RATE];

        Debug.Log(FireRate);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            ChangeProjectile();
        }
    }
    void FixedUpdate()
    {
        if(lastFireTime < Time.time - FireRate)
        {
            Fire();
            lastFireTime = Time.time;
        }
    }

    void Fire()
    {
        if(unusingProjectiles.Count > 0)
        {
            var projectile = unusingProjectiles[0];
            projectile.transform.position = muzzle.transform.position;
            projectile.SetActive(true);

            usingProjectiles.Add(projectile);
            unusingProjectiles.Remove(projectile);
        }
        else
        {
            var go = Instantiate(projectilePrefabs[currentProjectileIndex], muzzle.transform.position, Quaternion.identity);

            var projectile = go.GetComponent<Projectile>();
            projectile.range = playerStats.stats[CharacterColumn.Stat.FIRE_RANGE];
            projectile.speed = playerStats.stats[CharacterColumn.Stat.PROJECTILE_SPEED];

            usingProjectiles.Add(go);
        }
    }

    public void ReturnProjectile(GameObject projectile)
    {
        unusingProjectiles.Add(projectile);
        usingProjectiles.Remove(projectile);
    }

    private void ChangeProjectile()
    {
        currentProjectileIndex = (currentProjectileIndex + 1) % (projectilePrefabs.Count - 1);

        foreach(var item in usingProjectiles)
        {
            Destroy(item);
        }

        foreach (var item in unusingProjectiles)
        {
            Destroy(item);
        }

        usingProjectiles.Clear();
        unusingProjectiles.Clear();
    }
}
