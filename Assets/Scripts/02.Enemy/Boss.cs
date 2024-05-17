using System.Collections;
using UnityEngine;
using static Enemy;

public class Boss : Enemy
{
    private readonly string attack1 = "Attack1";
    private readonly string attack2 = "Attack2";
    private readonly string attack3 = "Attack3";

    private WaitForSeconds shotInterval = new WaitForSeconds(0.3f);
    public GameObject projectilePrefab;
    public Coroutine attackOneCoroutine;
    public EnemyState enemyState = EnemyState.Idle;
    public float projectileSpeed = 10;
    public float angle = 30f;
    private float distance = float.PositiveInfinity;
    [Tooltip("이 거리에 내에 들어올 때 공격")]
    public float attackDistance = 30;

    protected override void Start()
    {
        base.Start();
        base.UpdateStats(enemyData, 1, 0);
        StartCoroutine(AttackPattern());
    }

    private void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
                animator.SetBool(Animator.StringToHash(attack1), false);
                animator.SetBool(Animator.StringToHash(attack2), false);
                animator.SetBool(Animator.StringToHash(attack3), false);
                break;
            case EnemyState.Damaged:
                break;
            case EnemyState.Dead:
                break;
            case EnemyState.Attack1:
                break;
            case EnemyState.Attack2:
                break;
            case EnemyState.Attack3:
                break;
        }
    }

    public IEnumerator ShotThreeAngle()
    {
        enemyState = EnemyState.Attack1;
        animator.SetBool(Animator.StringToHash(attack1), true);
        animator.SetBool(Animator.StringToHash(attack2), false);
        animator.SetBool(Animator.StringToHash(attack3), false);

        int shotCount = 0;
        while(shotCount++ < 3)
        {
            var direction = (target.transform.position - transform.position).normalized;
            Shot(direction);
            yield return shotInterval;
        }
    }

    public void Shot(Vector3 direction)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = direction * projectileSpeed;
    }

    IEnumerator AttackPattern()
    {
        while (isAlive)
        {
            distance = Vector3.Distance(target.transform.position, transform.position);

            if (distance > attackDistance)
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            if (attackOneCoroutine != null)
            {
                StopCoroutine(attackOneCoroutine);
            }
            // 패턴 1
            yield return StartCoroutine(ShotThreeAngle());
            enemyState = EnemyState.Idle;
            yield return new WaitForSeconds(3);
            // 패턴 2

            // 패턴 3

        }
    }
}
