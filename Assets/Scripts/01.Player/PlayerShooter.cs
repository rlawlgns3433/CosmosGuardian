using System.Collections.Generic;
using UnityEngine;

public class PlayerShooter : MonoBehaviour
{
    public List<GameObject> usingProjectiles = new List<GameObject>();
    public List<GameObject> unusingProjectiles = new List<GameObject>();
    public List<GameObject> projectilePrefabs = new List<GameObject>();
    public GameObject muzzle;
    public PlayerStats playerStats;
    public int currentProjectileIndex = 0;
    [Tooltip("�Ѿ� �߻� ����")]
    public float spreadAngle = 30f;
    public Weapon weapon;
    private int oneMinute = 60;

    private float FireRate
    {
        get
        {
            return 1 / (((playerStats.stats[CharacterColumn.Stat.FIRE_RATE]) * weapon.stats[WeaponColumn.Stat.FIRE_RATE]) / oneMinute);
        }
    }
    public float speed = 20f;
    private float lastFireTime = 0f;

    private void Start()
    {
        if(!TryGetComponent(out weapon))
        {
            weapon.enabled = false;
            return;
        }

        currentProjectileIndex = weapon.weaponData.PROJECTILE_ID - 1;
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
        int projectileCount = (int)weapon.stats[WeaponColumn.Stat.PROJECTILE_AMOUNT];
        //float angleStep = projectileCount > 1 ? spreadAngle / (projectileCount - 1) : 0;
        // 2�� ���϶�� ���� ������
        // 3�� �̻��̶�� ū ������
        float tempAngle = 10f;

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
            float angle = startAngle + i * angleStep;

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
            if (unusingProjectiles.Count > 0)
            {
                projectile = unusingProjectiles[0];
                unusingProjectiles.RemoveAt(0);
            }
            else
            {
                projectile = Instantiate(projectilePrefabs[currentProjectileIndex], muzzle.transform.position, Quaternion.identity);
            }

            projectile.transform.position = muzzle.transform.position;
            projectile.transform.rotation = rotation;
            projectile.SetActive(true);
            projectile.GetComponent<Rigidbody>().velocity = projectileDirection * speed;

            usingProjectiles.Add(projectile);
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
