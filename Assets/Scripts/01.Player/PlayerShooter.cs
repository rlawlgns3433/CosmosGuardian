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
    public int currentProjectileIndex = 0;
    public float fireRate = 0.3f; // 임시 발사 간격
    public float speed = 20f;
    private float lastFireTime = 0f;
    private Vector3 direction = Vector3.forward;

    void Update()
    {
        if(lastFireTime < Time.time - fireRate)
        {
            Fire();
            lastFireTime = Time.time;
        }
    }

    void Fire()
    {
        if(unusingProjectiles.Count > 0)
        {
            // recycling
            var projectile = unusingProjectiles.First();
            projectile.SetActive(true);

            unusingProjectiles.Remove(projectile);
            usingProjectiles.Add(projectile);
        }
        else
        {
            var projectile = Instantiate(projectilePrefabs[currentProjectileIndex], muzzle.transform.position, Quaternion.identity);
            usingProjectiles.Add(projectile);
        }
    }

    public void ReturnProjectile(in GameObject projectile)
    {
        unusingProjectiles.Add(projectile);
        usingProjectiles.Remove(projectile);
    }
}
