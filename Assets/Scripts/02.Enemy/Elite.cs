using System.Collections;
using UnityEngine;

public class Elite : Enemy
{
    private readonly string attack1 = "Attack1";
    [Tooltip("Attack 1 발사 속도")]
    public float projectileSpeed = 10;
    [Tooltip("이 거리에 내에 들어올 때 공격")]
    public float attackDistance = 30;

    public GameObject projectilePrefab;
    public Coroutine attackOneCoroutine;
    private WaitForSeconds shotInterval = new WaitForSeconds(2f);
    private float distance = float.PositiveInfinity;

    protected override void Start()
    {
        base.Start();

        attackOneCoroutine = StartCoroutine(AttackPattern());
    }

    protected override void Update()
    {
        base.Update();
        switch (enemyState)
        {
            case EnemyState.Idle:
                animator.SetBool(Animator.StringToHash(attack1), false);
                break;
            case EnemyState.Attack1:
                break;
        }
    }

    private void Shot()
    {
        if (transform.position.z < target.transform.position.z) return;

        direction = (target.transform.position - transform.position).normalized;

        enemyState = EnemyState.Attack1;
        animator.SetBool(Animator.StringToHash(attack1), true);

        var shotPos = transform.position;
        shotPos += Vector3.up;
        GameObject go = Instantiate(projectilePrefab, shotPos, Quaternion.identity);
        Rigidbody rb = go.GetComponent<Rigidbody>();
        rb.velocity = -direction * projectileSpeed;

        var projectile = go.GetComponent<EliteBullet>();
        projectile.damage = enemyData.DAMAGE;
        Destroy(go, 5);
    }

    IEnumerator AttackPattern()
    {
        while(isAlive)
        {
            if (!target.isAlive)
            {
                if (attackOneCoroutine != null)
                {
                    StopCoroutine(attackOneCoroutine);
                }

                yield break;
            }

            distance = Vector3.Distance(target.transform.position, transform.position);

            if (distance > attackDistance)
            {
                yield return new WaitForSeconds(0.1f);
                continue;
            }

            if(attackOneCoroutine != null)
            {
                StopCoroutine(attackOneCoroutine);
            }

            yield return shotInterval;
            Shot();
            yield return new WaitForSeconds(1);
            enemyState = EnemyState.Idle;
            animator.SetBool(Animator.StringToHash(attack1), false);
        }
    }

}
