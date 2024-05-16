using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public List<GameObject> usingProjectiles = new List<GameObject>();
    public List<GameObject> unusingProjectiles = new List<GameObject>();
    public List<GameObject> projectilePrefabs = new List<GameObject>();
    public RuntimeAnimatorController[] animatorControllers;
    public GameObject muzzle;
    public PlayerStats playerStats;
    public int currentProjectileIndex = 0;
    public Weapon weapon;
    private int oneMinute = 60;
    private float FireRate
    {
        get
        {
            return 1 / (((playerStats.stats[CharacterColumn.Stat.FIRE_RATE]) * weapon.stats[WeaponColumn.Stat.FIRE_RATE]) / oneMinute);
            //return minFireRate - (0.4f * playerStats.stats[CharacterColumn.Stat.FIRE_RATE]) / maxFireRateLevel;
        }
    }
    public float speed = 20f;
    private float lastFireTime = 0f;

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
