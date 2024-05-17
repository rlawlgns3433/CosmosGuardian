using System.Collections;
using UnityEngine;

public class Boss : Enemy
{
    private WaitForSeconds shotInterval = new WaitForSeconds(0.3f);
    
    public GameObject projectilePrefab;
    public Coroutine attackOneCoroutine;
    public float projectileSpeed = 10;
    public float angle = 30f;

    protected override void Start()
    {
        base.Start();
        base.UpdateStats(enemyData, 1, 0);
        StartCoroutine(AttackPattern());
    }

    public IEnumerator ShotThreeAngle()
    {
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
            if (attackOneCoroutine != null)
            {
                StopCoroutine(attackOneCoroutine);
            }
            // 패턴 1
            yield return StartCoroutine(ShotThreeAngle());
            yield return new WaitForSeconds(3);
            // 패턴 2

            // 패턴 3

        }
    }
}
