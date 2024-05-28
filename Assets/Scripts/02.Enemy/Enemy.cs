using System;
using System.Collections;
using TMPro;
using UnityEngine;

public enum EnemyType
{
    Bat = 40000, // �����̴� ���� (Chase Ȱ��)
    Dragon = 40001, // �����̴� ���� (Chase Ȱ��)
    Elite = 40100,
    MidBoss = 40200,
    Boss = 40300
}

public class Enemy : MonoBehaviour, IDamageable
{
    public enum EnemyState
    {
        Idle,
        Damaged,
        Dead,
        Attack1,
        Attack2,
        Attack3
    }

    public EnemyState enemyState = EnemyState.Idle;
    public EnemyType enemyType;
    public Coroutine chaseCoroutine;
    public TextMeshProUGUI textHealth;
    public event Action onDeath;
    public float originSpeed = 5;
    public float speed = 5;
    public bool isAlive = true;
    public bool isChasing = false;
    public float rotationSpeed = 180;
    public Animator animator;
    protected ItemController itemController;
    public EnemyData enemyData = null;
    public PlayerHealth target = null;
    public Vector3 damageFloatingPosition;
    public float floatingTextRadius;
    public SphereCollider sphereCollider;
    public GameObject splashEffect;
    public ParticleSystem[] effects;
    private WaitForSeconds sec = new WaitForSeconds(0.5f);
    private Coroutine stopEffectCoroutine;


    private WaitForSeconds chaseTimer;
    public Vector3 direction;

    private void Awake()
    {
        if (!TryGetComponent(out animator))
        {
            animator.enabled = false;
            return;
        }
        effects = splashEffect.GetComponentsInChildren<ParticleSystem>();
    }

    protected virtual void OnEnable()
    {
        isAlive = true;
        speed = originSpeed;
        damageFloatingPosition = textHealth.transform.position;

        target = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerHealth>();
        direction = (target.transform.position - transform.position).normalized;

    }

    protected virtual void OnDisable()
    {
        if (chaseCoroutine != null)
        {
            StopCoroutine(chaseCoroutine);
        }

        isAlive = false;
        isChasing = false;
        textHealth.gameObject.SetActive(false);
    }

    protected virtual void Start()
    {
        if (enemyData == null)
        {
            enemyData = new EnemyData(GameManager.Instance.enemyTable.Get((int)(enemyType)));
        }

        chaseTimer = new WaitForSeconds(0.1f);

        floatingTextRadius = 3f;
        onDeath += () =>
        {
            isAlive = false;
            if (target.playerStats.stats[CharacterColumn.Stat.HP] <= 0) return;
            if (target.camShakeCoroutine != null)
            {
                target.StopCoroutine(target.camShakeCoroutine);
            }
        };
        onDeath += OnDie;
        onDeath += () => { target.GetComponent<PlayerStats>().GetExp(enemyData.SCORE); };
    }

    protected virtual void Update()
    {
        switch (enemyType)
        {
            case EnemyType.Bat:
            case EnemyType.Elite:
                if (isChasing)
                {
                    transform.Translate(direction * speed * Time.deltaTime, Space.World);
                    Vector3 directionToTarget = target.transform.position - transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
                }
                break;
        }
    }

    public void OnDamage(float damage, bool isCritical = false, Vector3 hitPoint = default, Vector3 hitNormal = default)
    {
        if (!isAlive) return;

        if (isCritical)
        {
            DynamicTextManager.CreateText(damageFloatingPosition + UnityEngine.Random.onUnitSphere * sphereCollider.radius / 2, Mathf.RoundToInt(damage).ToString(), DynamicTextManager.criticalDamageData);
        }
        else
        {
            DynamicTextManager.CreateText(damageFloatingPosition + UnityEngine.Random.onUnitSphere * sphereCollider.radius / 2, Mathf.RoundToInt(damage).ToString(), DynamicTextManager.normalDamgeData);
        }

        enemyData.HP -= damage;
        if (enemyData.HP <= 0)
        {
            enemyData.HP = 0;
            onDeath();
        }

        textHealth.text = Mathf.CeilToInt(enemyData.HP).ToString();
    }

    public void OnDie()
    {
        isAlive = false;
        isChasing = false;
        enemyState = EnemyState.Dead;

        textHealth.gameObject.SetActive(false);
        sphereCollider.enabled = false;

        animator.SetTrigger(Animator.StringToHash("Die"));
        speed = 0;
        // ���� ��� Ǯ�� �̵�
        Invoke("LateSetActiveFalse", 1.5f);
        //Destroy(gameObject, 1.5f);
    }


    public IEnumerator CoChasePlayer()
    {
        isChasing = true;

        while (isAlive && target.isAlive && isChasing)
        {
            yield return chaseTimer;

            direction = (target.transform.position - transform.position).normalized;
        }

        if (!target.isAlive)
        {
            isChasing = false;
            direction = Vector3.zero;
        }
    }

    public void Chase()
    {
        if (chaseCoroutine != null)
        {
            StopCoroutine(chaseCoroutine);
        }

        if (isAlive && gameObject.activeInHierarchy)
        {
            chaseCoroutine = StartCoroutine(CoChasePlayer());
        }
    }

    //public void UpdateStats(EnemyData enemyData, float hpScale, int resetCount)
    //{
    //    this.enemyData.TYPE = enemyData.TYPE;
    //    this.enemyData.DAMAGE = enemyData.DAMAGE;
    //    this.enemyData.SCORE = enemyData.SCORE;
    //    this.enemyData.GOLD = enemyData.GOLD;
    //    this.enemyData.MONSTER_ID = enemyData.MONSTER_ID;
    //    this.enemyData.HP = enemyData.HP * Mathf.Pow(hpScale, resetCount);

    //    textHealth.text = ((int)this.enemyData.HP).ToString();
    //}

    public virtual void UpdateStats(EnemyData enemyData, float magnification, int resetCount)
    {
        this.enemyData = new EnemyData(enemyData); // ����� ������ ���
        this.enemyData.HP *= Mathf.Pow(magnification, resetCount);
        this.enemyData.DAMAGE *= Mathf.Pow(magnification, resetCount);

        textHealth.text = ((int)this.enemyData.HP).ToString();
    }

    public void StopEffectImmediatly()
    {
        foreach (var particle in effects)
        {
            particle.Stop();
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        splashEffect.SetActive(false);
    }

    public void StopEffect()
    {
        if (stopEffectCoroutine != null)
        {
            StopCoroutine(stopEffectCoroutine);
        }

        stopEffectCoroutine = StartCoroutine(StopEffectAfter(sec));
    }

    IEnumerator StopEffectAfter(WaitForSeconds sec)
    {
        yield return sec;

        if (effects[0].isPlaying)
        {
            StopEffectImmediatly();
        }
    }

    public void LateSetActiveFalse()
    {

        gameObject.SetActive(false);
    }
}
