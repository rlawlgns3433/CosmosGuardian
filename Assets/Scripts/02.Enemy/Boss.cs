using System.Collections;
using UnityEngine;

public class Boss : Enemy
{
    private readonly string attack1 = "Attack1";
    private readonly string attack2 = "Attack2";
    private readonly string attack3 = "Attack3";

    [Tooltip("Attack 1 �߻� ����")]
    public float intervalFloat = 0.3f;
    private WaitForSeconds shotInterval;
    public GameObject projectilePrefab;
    public Coroutine attackOneCoroutine;
    public EnemyState enemyState = EnemyState.Idle;
    [Tooltip("Attack 1 �߻� �ӵ�")]
    public float projectileSpeed = 10;
    public float angle = 30f;
    private float distance = float.PositiveInfinity;
    [Tooltip("�� �Ÿ��� ���� ���� �� ����")]
    public float attackDistance = 30;

    private float savedVerticalSpeed;

    protected override void Start()
    {
        base.Start();
        base.UpdateStats(enemyData, 1, 0);
        shotInterval = new WaitForSeconds(intervalFloat);

        StartCoroutine(AttackPattern());
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, target.transform.position) < 12)
        {
            savedVerticalSpeed = target.gameObject.GetComponent<PlayerStats>().stats[CharacterColumn.Stat.MOVE_SPEED_V];
            target.gameObject.GetComponent<PlayerStats>().stats[CharacterColumn.Stat.MOVE_SPEED_V] = 0f;
        }

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
                target.gameObject.GetComponent<PlayerStats>().stats[CharacterColumn.Stat.MOVE_SPEED_V] = savedVerticalSpeed;
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
        var shotPos = transform.position;
        shotPos += (Vector3.up * 3);
        GameObject projectile = Instantiate(projectilePrefab, shotPos, Quaternion.identity);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = (direction - Vector3.up * 0.1f) * projectileSpeed;

        Destroy(projectile, 5f);
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
            // ���� 1
            yield return StartCoroutine(ShotThreeAngle());
            enemyState = EnemyState.Idle;
            yield return new WaitForSeconds(3);
            // ���� 2

            // ���� 3

        }
    }
}
