using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [Tooltip("총알이 사라지기까지 걸리는 시간")]
    public float disappearTimer = 1f;
    public Vector3 startPosition = Vector3.zero;
    public Rigidbody rb;
    public AudioSource audioSource;

    [SerializeField] protected float speed = 15f;
    [SerializeField] protected float hitOffset = 0f;
    [SerializeField] protected bool UseFirePointRotation;
    [SerializeField] protected Vector3 rotationOffset = Vector3.zero;
    [SerializeField] protected GameObject hit;
    [SerializeField] protected ParticleSystem hitPS;
    [SerializeField] protected GameObject flash;
    [SerializeField] protected Collider col;
    [SerializeField] protected Light lightSourse;
    [SerializeField] protected GameObject[] Detached;
    [SerializeField] protected ParticleSystem projectilePS;
    [SerializeField] protected bool notDestroy = false;

    private Coroutine returnCoroutine = null;
    private PlayerShooter playerShooter;
    //private PlayerStats playerStats;
    private PlayerHealth playerHealth;
    private float rand;
    private Collider[] splashDamageColliders = new Collider[10]; // 미리 할당된 배열
    private bool startChecker = false;

    // =======Range
    public float rangeScale;
    public float weaponRange;
    public float Range => rangeScale * weaponRange;

    // =======Projectile Speed
    public float speedScale;
    public float weaponSpeed;
    public float Speed => speedScale * weaponSpeed;

    // =======Critical Rate
    public float criticalRateScale;
    public float weaponCriticalRate;
    public float CriticalRate => criticalRateScale * (weaponCriticalRate / 100);

    // =======Critical Damage
    public float criticalDamageScale;
    public float weaponCriticalDamage;
    public float CriticalDamage => criticalDamageScale * (weaponCriticalDamage / 100) * Damage;

    // =======Damage
    public float damageScale;
    public float weaponDamage;
    public float Damage => (damageScale * weaponDamage) / (playerShooter.playerStats.stats[CharacterColumn.Stat.PROJECTILE_AMOUNT] * playerShooter.weapon.stats[WeaponColumn.Stat.PROJECTILE_AMOUNT]);

    // =======HpDrain
    public float hpDrainScale;
    public float weaponHpDrain;
    public float HpDrain => rand <= CriticalRate ? hpDrainScale * (weaponHpDrain / 100) * CriticalDamage : hpDrainScale * (weaponHpDrain / 100) * Damage;

    // =======SplashDamage
    public float splashDamageScale;
    public float weaponSplashDamage;
    public float SplashDamage => splashDamageScale * (weaponSplashDamage / 100) * Damage;

    // =======SplashDamageRange
    public float splashDamageRangeScale;
    public float weaponSplashDamageRange;
    public float SplashDamageRange => splashDamageRangeScale * weaponSplashDamageRange;

    // =======Penetrate
    public float penetrateScale;
    public float weaponPenetrate;
    public int Penetrate => Mathf.RoundToInt(penetrateScale * weaponPenetrate);

    // =======CurrentPenetrate
    public int currentPenetrate;
    public int CurrentPenetrate
    {
        get => Penetrate - currentPenetrate;
        set => currentPenetrate = value;
    }

    private void Awake()
    {
        audioSource.volume = ParamManager.SfxValue;
        if (playerShooter == null && !GameObject.FindWithTag(Tags.Player).TryGetComponent(out playerShooter))
        {
            playerShooter.enabled = false;
        }

        if (playerShooter.playerStats == null && !GameObject.FindWithTag(Tags.Player).TryGetComponent(out playerShooter.playerStats))
        {
            playerShooter.playerStats.enabled = false;
        }

        if (playerHealth == null && !GameObject.FindWithTag(Tags.Player).TryGetComponent(out playerHealth))
        {
            playerHealth.enabled = false;
        }
    }

    private void OnEnable()
    {
        ResetProjectile();
        InitializeProjectileStats();

        if (flash != null) flash.transform.parent = null;
        if (lightSourse != null) lightSourse.enabled = true;
        col.enabled = true;
        rb.constraints = RigidbodyConstraints.None;
        startPosition = rb.position = playerShooter.muzzle.transform.position;
        rb.velocity = playerShooter.muzzle.transform.forward * Speed; // 초기 속도 설정
        startChecker = true;

        PlayAudio(SoundManager.Instance.flashClips[playerShooter.weapon.weaponData.PROJECTILE_ID - 1]);
    }

    private void ResetProjectile()
    {
        hitPS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        projectilePS.Play();
        currentPenetrate = 0;
    }

    private void InitializeProjectileStats()
    {
        rangeScale = playerShooter.playerStats.stats[CharacterColumn.Stat.FIRE_RANGE];
        weaponRange = playerShooter.weapon.stats[WeaponColumn.Stat.FIRE_RANGE];
        speedScale = playerShooter.playerStats.stats[CharacterColumn.Stat.PROJECTILE_SPEED];
        weaponSpeed = playerShooter.weapon.stats[WeaponColumn.Stat.PROJECTILE_SPEED];
        criticalRateScale = playerShooter.playerStats.stats[CharacterColumn.Stat.CRITICAL];
        weaponCriticalRate = playerShooter.weapon.stats[WeaponColumn.Stat.CRITICAL];
        criticalDamageScale = playerShooter.playerStats.stats[CharacterColumn.Stat.CRITICAL_DAMAGE];
        weaponCriticalDamage = playerShooter.weapon.stats[WeaponColumn.Stat.CRITICAL_DAMAGE];
        damageScale = playerShooter.playerStats.stats[CharacterColumn.Stat.DAMAGE];
        weaponDamage = playerShooter.weapon.stats[WeaponColumn.Stat.DAMAGE];
        hpDrainScale = playerShooter.playerStats.stats[CharacterColumn.Stat.HP_DRAIN];
        weaponHpDrain = playerShooter.weapon.stats[WeaponColumn.Stat.HP_DRAIN];
        splashDamageScale = playerShooter.playerStats.stats[CharacterColumn.Stat.SPLASH_DAMAGE];
        weaponSplashDamage = playerShooter.weapon.stats[WeaponColumn.Stat.SPLASH_DAMAGE];
        splashDamageRangeScale = playerShooter.playerStats.stats[CharacterColumn.Stat.SPLASH_RANGE];
        weaponSplashDamageRange = playerShooter.weapon.stats[WeaponColumn.Stat.SPLASH_RANGE];
        penetrateScale = playerShooter.playerStats.stats[CharacterColumn.Stat.PENETRATE];
        weaponPenetrate = playerShooter.weapon.stats[WeaponColumn.Stat.PENETRATE];
    }

    private void PlayAudio(AudioClip clip)
    {
        if (!audioSource.isPlaying)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    private void Update()
    {
        if (Vector3.Distance(startPosition, transform.position) >= Range)
        {
            DeactivateProjectile();
        }
    }

    private void DeactivateProjectile()
    {
        gameObject.SetActive(false);
        playerShooter.ReturnProjectile(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Enemy) || other.CompareTag(Tags.Boss) || other.CompareTag(Tags.Elite))
        {
            HandleCollisionWithEnemy(other);
        }
    }

    private void HandleCollisionWithEnemy(Collider other)
    {
        var enemy = other.gameObject.GetComponent<Enemy>();
        rand = Random.value;

        if (rand <= CriticalRate)
        {
            enemy.OnDamage(CriticalDamage, true);
        }
        else
        {
            enemy.OnDamage(Damage);
        }

        ApplySplashDamage(other);
        ApplyHpDrain();

        if (CurrentPenetrate <= 0)
        {
            HandleProjectileHit(other);
        }
        ++currentPenetrate;
    }

    private void ApplySplashDamage(Collider other)
    {
        if (SplashDamageRange > 0)
        {
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, SplashDamageRange, splashDamageColliders);

            for (int i = 0; i < numColliders; i++)
            {
                var collider = splashDamageColliders[i];
                if (collider == other) continue;

                if (collider.CompareTag(Tags.Enemy) || collider.CompareTag(Tags.Boss))
                {
                    var e = collider.gameObject.GetComponent<Enemy>();
                    e.OnDamage(SplashDamage);
                    PlaySplashEffect(e);
                }
            }
        }
    }

    private void PlaySplashEffect(Enemy enemy)
    {
        if (enemy.effects[0].isPlaying)
        {
            enemy.StopEffectImmediatly();
        }

        enemy.splashEffect.SetActive(true);
        foreach (var particle in enemy.effects)
        {
            particle.Play();
            enemy.StopEffect();
        }
    }

    private void ApplyHpDrain()
    {
        if (HpDrain > 0)
        {
            DynamicTextManager.CreateText(playerHealth.transform.position + Vector3.one * 2, HpDrain.ToString("+#;"), DynamicTextManager.healingTextData);
            playerShooter.playerStats.stats[CharacterColumn.Stat.HP] += HpDrain;
            playerHealth.UpdateHealthUI();
        }
    }

    private void HandleProjectileHit(Collider other)
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        if (lightSourse != null) lightSourse.enabled = false;


        if (lightSourse != null) lightSourse.enabled = false;
        col.enabled = false;
        projectilePS.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        Vector3 contact = other.ClosestPoint(transform.position);
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, (transform.position - contact).normalized);
        Vector3 pos = contact + contact * hitOffset;

        if (hit != null)
        {
            SetHitEffectPositionAndRotation(contact, rot, pos);
            PlayHitEffect();
        }

        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                ParticleSystem detachedPS = detachedPrefab.GetComponent<ParticleSystem>();
                detachedPS.Stop();
            }
        }

        startChecker = false;

        if (returnCoroutine != null)
        {
            StopCoroutine(returnCoroutine);
        }

        returnCoroutine = StartCoroutine(ReturnProjectileAfter(disappearTimer));
    }

    private void SetHitEffectPositionAndRotation(Vector3 contact, Quaternion rot, Vector3 pos)
    {
        hit.transform.rotation = rot;
        hit.transform.position = pos;

        if (UseFirePointRotation)
        {
            hit.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0);
        }
        else if (rotationOffset != Vector3.zero)
        {
            hit.transform.rotation = Quaternion.Euler(rotationOffset);
        }
        else
        {
            hit.transform.LookAt((transform.position - contact).normalized + (transform.position - contact).normalized);
        }
    }

    private void PlayHitEffect()
    {
        hitPS.Play();
        PlayAudio(SoundManager.Instance.hitClips[playerShooter.weapon.weaponData.PROJECTILE_ID - 1]);
    }

    IEnumerator ReturnProjectileAfter(float t)
    {
        yield return new WaitForSeconds(t);
        gameObject.SetActive(false);
        playerShooter.ReturnProjectile(gameObject);
    }
}
