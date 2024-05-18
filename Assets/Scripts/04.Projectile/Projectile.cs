using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private PlayerShooter playerShooter;
    private PlayerStats playerStats;

    [SerializeField] protected float speed = 15f;
    [SerializeField] protected float hitOffset = 0f;
    [SerializeField] protected bool UseFirePointRotation;
    [SerializeField] protected Vector3 rotationOffset = new Vector3(0, 0, 0);
    [SerializeField] protected GameObject hit;
    [SerializeField] protected ParticleSystem hitPS;
    [SerializeField] protected GameObject flash;
    //[SerializeField] protected Rigidbody rb;
    [SerializeField] protected Collider col;
    [SerializeField] protected Light lightSourse;
    [SerializeField] protected GameObject[] Detached;
    [SerializeField] protected ParticleSystem projectilePS;
    private bool startChecker = false;
    [SerializeField] protected bool notDestroy = false;

    Coroutine returnCoroutine = null;
    [Tooltip("총알이 사라지기까지 걸리는 시간")]
    public float disappearTimer = 1f;
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
        //--===================================

        hitPS.Stop();
        hitPS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        projectilePS.Play();

        if (!startChecker)
        {
            if (flash != null)
            {
                flash.transform.parent = null;
            }
            if (lightSourse != null)
                lightSourse.enabled = true;
            col.enabled = true;
            rigidbody.constraints = RigidbodyConstraints.None;
        }
        startPosition = rigidbody.position = playerShooter.muzzle.transform.position;
        rigidbody.velocity = Vector3.forward * speed;

        startChecker = true;
        //--===================================

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


            //==================================

            //Lock all axes movement and rotation
            rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            //speed = 0;
            if (lightSourse != null)
                lightSourse.enabled = false;
            col.enabled = false;
            projectilePS.Stop();
            projectilePS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

            Vector3 contact = other.ClosestPoint(transform.position);
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, (transform.position - contact).normalized);
            Vector3 pos = contact + contact * hitOffset;

            //Spawn hit effect on collision
            if (hit != null)
            {
                hit.transform.rotation = rot;
                hit.transform.position = pos;
                if (UseFirePointRotation) { hit.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
                else if (rotationOffset != Vector3.zero) { hit.transform.rotation = Quaternion.Euler(rotationOffset); }
                else { hit.transform.LookAt((transform.position - contact).normalized + (transform.position - contact).normalized); }
                hitPS.Play();
            }

            //Removing trail from the projectile on cillision enter or smooth removing. Detached elements must have "AutoDestroying script"
            foreach (var detachedPrefab in Detached)
            {
                if (detachedPrefab != null)
                {
                    ParticleSystem detachedPS = detachedPrefab.GetComponent<ParticleSystem>();
                    detachedPS.Stop();
                }
            }

            startChecker = false;

            //==================================

            //gameObject.SetActive(false);
            //playerShooter.ReturnProjectile(gameObject);
            
            if(returnCoroutine != null)
            {
                StopCoroutine(returnCoroutine);
            }

            returnCoroutine = StartCoroutine(ReturnProjectileAfter(disappearTimer));
        }
    }

    IEnumerator ReturnProjectileAfter(float t)
    {
        yield return new WaitForSeconds(t);
        gameObject.SetActive(false);
        playerShooter.ReturnProjectile(gameObject);
    }
}
