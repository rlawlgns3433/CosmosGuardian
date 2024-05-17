using UnityEngine;

public class Projectile : MonoBehaviour
{
    private PlayerShooter playerShooter;
    private PlayerStats playerStats;

    // =======Range
    public float rangeScale;
    public float weaponRange;
    public float Range
    {
        get
        {
            return rangeScale * weaponRange;
        }
    }

    // =======Projectile Speed
    public float speedScale;
    public float weaponSpeed;
    public float Speed
    {
        get
        {
            return speedScale * weaponSpeed;
        }
    }


    // =======Critical Rate
    public float criticalRateScale;
    public float weaponCriticalRate;
    public float CriticalRate
    {
        get
        {
            return criticalRateScale * weaponCriticalRate;
        }
    }

    // =======Critical Damage
    public float criticalDamageScale;
    public float weaponCriticalDamage;
    public float CriticalDamage
    {
        get
        {
            return criticalDamageScale * weaponCriticalDamage;
        }
    }

    // =======Damage
    public float damageScale;
    public float weaponDamage;
    public float Damage
    {
        get
        {
            return damageScale * weaponDamage;
        }
    }


    public float splashDamage;
    public float splashDamageRange;
    public int penetrationCount;
    public Vector3 startPosition = Vector3.zero;
    public Rigidbody rigidbody;

    private void OnEnable()
    {
        if (playerShooter == null && !GameObject.FindWithTag(Tags.Player).TryGetComponent(out playerShooter))
        {
            playerShooter.enabled = false;
            return;
        }

        if (playerStats == null && !GameObject.FindWithTag(Tags.Player).TryGetComponent(out playerStats))
        {
            playerStats.enabled = false;
            return;
        }

        rangeScale = playerStats.stats[CharacterColumn.Stat.FIRE_RANGE];
        weaponRange = playerShooter.weapon.stats[WeaponColumn.Stat.FIRE_RANGE];

        speedScale = playerStats.stats[CharacterColumn.Stat.PROJECTILE_SPEED];
        weaponSpeed = playerShooter.weapon.stats[WeaponColumn.Stat.PROJECTILE_SPEED];

        criticalRateScale = playerStats.stats[CharacterColumn.Stat.CRITICAL];
        weaponCriticalRate = playerShooter.weapon.stats[WeaponColumn.Stat.CRITICAL];

        criticalDamageScale = playerStats.stats[CharacterColumn.Stat.CRITICAL_DAMAGE];
        weaponCriticalDamage = playerShooter.weapon.stats[WeaponColumn.Stat.CRITICAL_DAMAGE];

        damageScale = playerStats.stats[CharacterColumn.Stat.DAMAGE];
        weaponDamage = playerShooter.weapon.stats[WeaponColumn.Stat.DAMAGE];

        splashDamage = playerStats.stats[CharacterColumn.Stat.SPLASH_DAMAGE];

        splashDamageRange = playerStats.stats[CharacterColumn.Stat.SPLASH_RANGE];

        penetrationCount = (int)playerStats.stats[CharacterColumn.Stat.PENETRATE];

        startPosition = playerShooter.muzzle.transform.position;
        transform.position = startPosition;

        rigidbody.velocity = Vector3.zero;  // Rigidbody 속도 초기화
        rigidbody.angularVelocity = Vector3.zero; // 각속도 초기화
    }

    private void Update()
    {
        rigidbody.AddForce(transform.forward * Speed * Time.deltaTime, ForceMode.Impulse);

        float distance = Vector3.Distance(startPosition, transform.position);

        if (distance >= Range)
        {
            gameObject.SetActive(false);
            playerShooter.ReturnProjectile(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tags.Enemy) || other.CompareTag(Tags.Boss))
        {
            var enemy = other.gameObject.GetComponent<Enemy>();
            enemy.OnDamage(Damage);

            gameObject.SetActive(false);
            playerShooter.ReturnProjectile(gameObject);
        }
    }
}
