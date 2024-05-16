using System;
using System.Collections;
using TMPro;
using UnityEngine;

// 스테이지가 진행될수록 강력해진다.

/// <summary>
/// 스코어
/// 재화
/// 경험치
/// HP
/// 이동속도
/// </summary>

public enum EnemyType
{
    Normal = 40000,
    Elite = 40100,
    MidBoss = 40200,
    Boss = 40300
}

public class Enemy : MonoBehaviour, IDamageable
{
    public EnemyType enemyType;
    public EnemyTable enemyTable;
    public Coroutine chaseCoroutine;
    public TextMeshProUGUI textHealth;
    public event Action onDeath;
    public float speed = 5;
    private bool isAlive = true;
    public bool isChasing = false;
    public float rotationSpeed = 180;
    private Animator animator;
    public EnemyData enemyData;
    public PlayerHealth target = null;
    private WaitForSeconds chaseTimer = new WaitForSeconds(1f);
    //public Vector3 direction;

    private void Awake()
    {
        if(!TryGetComponent(out animator))
        {
            animator.enabled = false;
            return;
        }
    }

    private void OnEnable()
    {
        enemyTable = DataTableMgr.Get<EnemyTable>(DataTableIds.Enemy);
        enemyData = enemyTable.Get((int)(enemyType));

        target = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerHealth>();
    }

    private void Start()
    {
        onDeath += OnDie;
        onDeath += () => { target.GetComponent<PlayerStats>().GetExp(enemyData.SCORE); };

        //direction = (target.transform.position - transform.position).normalized;
    }

    //private void Update()
    //{
    //    if(isChasing)
    //    {
    //        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    //        Vector3 directionToTarget = target.transform.position - transform.position;
    //        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
    //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    //    }
    //}

    public void OnDamage(float damage, Vector3 hitPoint = default, Vector3 hitNormal = default)
    {
        if (!isAlive) return;

        enemyData.HP -= damage;
        if (enemyData.HP <= 0)
        {
            enemyData.HP = 0;
            onDeath();
        }

        textHealth.text = ((int)enemyData.HP).ToString();
    }

    public void OnDie()
    {
        isAlive = false;
        isChasing = false;

        Collider[] colliders = GetComponents<Collider>();
        foreach(var collider in colliders)
        {
            collider.enabled = false;
        }

        animator.SetTrigger("Die");
        Destroy(gameObject, 3f);
        speed = 0;
    }

    //public IEnumerator CoChasePlayer()
    //{
    //    isChasing = true;

    //    while(isAlive && target.isAlive && isChasing)
    //    {
    //        yield return chaseTimer;
            
    //        direction = (target.transform.position - transform.position).normalized;
    //    }

    //    if(!target.isAlive)
    //    {
    //        direction = Vector3.zero;
    //    }
    //}

    //public void Chase()
    //{
    //    if(chaseCoroutine != null)
    //    {
    //        StopCoroutine(chaseCoroutine);
    //    }
    //    chaseCoroutine = StartCoroutine(CoChasePlayer());
    //}

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

    public void UpdateStats(EnemyData enemyData, float hpScale, int resetCount)
    {
        this.enemyData = new EnemyData(enemyData); // 복사된 데이터 사용
        this.enemyData.HP *= Mathf.Pow(hpScale, resetCount);

        textHealth.text = ((int)this.enemyData.HP).ToString();
    }

}
