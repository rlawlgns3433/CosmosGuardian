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
    private float currentFireRateLevel;
    private float FireRate
    {
        get
        {
            return minFireRate - (0.4f * currentFireRateLevel) / 40;
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
            var projectile = Instantiate(projectilePrefabs[currentProjectileIndex], muzzle.transform.position, Quaternion.identity);
            usingProjectiles.Add(projectile);
        }
    }

    public void ReturnProjectile(GameObject projectile)
    {
        unusingProjectiles.Add(projectile);
        usingProjectiles.Remove(projectile);
    }
}
