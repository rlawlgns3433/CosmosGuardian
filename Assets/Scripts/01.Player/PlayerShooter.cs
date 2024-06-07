using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public WeaponColumn.ProjectileType[] types;

    public Dictionary<WeaponColumn.ProjectileType, List<GameObject>> usingProjectiles = new Dictionary<WeaponColumn.ProjectileType, List<GameObject>>();
    public Dictionary<WeaponColumn.ProjectileType, List<GameObject>> unusingProjectiles = new Dictionary<WeaponColumn.ProjectileType, List<GameObject>>();
    public List<GameObject> projectilePrefabs = new List<GameObject>();

    public GameObject muzzle;
    public PlayerStats playerStats;
    public Weapon weapon;
    [Tooltip("총알 발사 각도")]
    public float spreadAngle = 20f;
    private float tempAngle = 10f;
    private int oneMinute = 60;
    public int currentProjectileIndex = 0;
    public float speed = 20f;
    private float lastFireTime = 0f;

    private float FireRate
    {
        get
        {
            return 1 / (((playerStats.stats[CharacterColumn.Stat.FIRE_RATE]) * weapon.stats[WeaponColumn.Stat.FIRE_RATE]) / oneMinute);
        }
    }

    private void Awake()
    {
        types = new WeaponColumn.ProjectileType[]
        {
            WeaponColumn.ProjectileType.Acid,
            WeaponColumn.ProjectileType.Torpedo,
            WeaponColumn.ProjectileType.Blackhole,
            WeaponColumn.ProjectileType.Fire,
            WeaponColumn.ProjectileType.Thunder,
            WeaponColumn.ProjectileType.Bullet,
            WeaponColumn.ProjectileType.Blue,
            WeaponColumn.ProjectileType.Huricara,
            WeaponColumn.ProjectileType.Arrow
        };

        foreach (var type in types)
        {
            usingProjectiles[type] = new List<GameObject>();
            unusingProjectiles[type] = new List<GameObject>();
        }
    }

    private void Start()
    {
        currentProjectileIndex = weapon.weaponData.PROJECTILE_ID - 1;
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
        int projectileCount = Mathf.RoundToInt(weapon.stats[WeaponColumn.Stat.PROJECTILE_AMOUNT] * playerStats.stats[CharacterColumn.Stat.PROJECTILE_AMOUNT]);

        float angleStep;

        if (projectileCount > 1)
        {
            if (projectileCount == 2)
            {
                angleStep = tempAngle;
            }
            else
            {
                angleStep = spreadAngle / (projectileCount - 1);
            }
        }
        else
        {
            angleStep = 0;
        }

        float startAngle = -spreadAngle / 2;

        for (int i = 0; i < projectileCount; i++)
        {
            float angle;

            if(projectileCount == 2)
            {
                if(i == 0)
                {
                    angle = -tempAngle / 2;   
                }
                else
                {
                    angle = tempAngle / 2;
                }
            }
            else
            {
                angle = startAngle + i * angleStep;

                if (projectileCount % 2 == 1 && i == projectileCount / 2)
                {
                    angle = 0;
                }
            }

            Quaternion rotation = Quaternion.Euler(0, angle, 0);
            Vector3 projectileDirection = rotation * muzzle.transform.forward;

            GameObject projectile;
            WeaponColumn.ProjectileType type = (WeaponColumn.ProjectileType)(currentProjectileIndex + 1);
            if (unusingProjectiles[type].Count > 0)
            {
                projectile = unusingProjectiles[type][0];
                unusingProjectiles[type].RemoveAt(0);
            }
            else
            {
                projectile = Instantiate(projectilePrefabs[currentProjectileIndex], muzzle.transform.position, Quaternion.identity);
            }

            projectile.transform.position = muzzle.transform.position;
            projectile.transform.rotation = rotation;
            projectile.SetActive(true);
            projectile.GetComponent<Rigidbody>().velocity = projectileDirection * speed;

            usingProjectiles[type].Add(projectile);
        }
    }

    public void ReturnProjectile(WeaponColumn.ProjectileType type, GameObject projectile)
    {
        unusingProjectiles[type].Add(projectile);
        usingProjectiles[type].Remove(projectile);
    }
}
