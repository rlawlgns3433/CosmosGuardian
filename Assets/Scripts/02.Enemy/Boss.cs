using System.Collections;
using UnityEngine;

public class Boss : Enemy
{
    public static bool IsBossDead = false; // 보스 사망 플래그

    private readonly string attack1 = "Attack1";
    private readonly string attack2 = "Attack2";
    private readonly string attack3 = "Attack3";

    [Tooltip("Attack 1 발사 간격")]
    public float intervalFloat = 0.3f;
    [Tooltip("Attack 1 발사 속도")]
    public float projectileSpeed = -10;
    [Tooltip("이 거리에 내에 들어올 때 공격")]
    public float attackDistance = 30;
    public GameObject projectilePrefab;
    public Coroutine attackOneCoroutine;
    public float angle = 30f;

    private Platform bossPlatform;
    private CameraMove cameraMove;
    private WaitForSeconds shotInterval;
    private float distance = float.PositiveInfinity;
    private float savedVerticalSpeed;

    protected override void Start()
    {
        base.Start();

        if (!GameObject.FindWithTag(Tags.BossPlatform).TryGetComponent(out bossPlatform))
        {
            bossPlatform.enabled = false;
            return;
        }

        onDeath += () =>
        {
            IsBossDead = true;

            var uiController = GameObject.FindWithTag(Tags.UiController).GetComponent<UiController>();
            uiController.item.SetActive(true);

            StopAllCoroutines();
            cameraMove.IsTOP = true;
            target.gameObject.GetComponent<PlayerStats>().stats[CharacterColumn.Stat.MOVE_SPEED_V] = savedVerticalSpeed;

            itemController = GameObject.FindWithTag(Tags.ItemController).GetComponent<ItemController>();
            itemController.UpdateItemData(enemyData.TYPE);
            GameObject.FindWithTag(Tags.Joystick).SetActive(false);
            Time.timeScale = 0f;
        };

        shotInterval = new WaitForSeconds(intervalFloat);
        if (!Camera.main.TryGetComponent(out cameraMove))
        {
            cameraMove.enabled = false;
            return;
        }
        savedVerticalSpeed = target.gameObject.GetComponent<PlayerStats>().stats[CharacterColumn.Stat.MOVE_SPEED_V];
        StartCoroutine(AttackPattern());
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < 12)
        {
            if (isAlive && cameraMove.IsTOP)
            {
                cameraMove.IsTOP = !cameraMove.IsTOP;
                savedVerticalSpeed = target.gameObject.GetComponent<PlayerStats>().stats[CharacterColumn.Stat.MOVE_SPEED_V];
                target.gameObject.GetComponent<PlayerStats>().stats[CharacterColumn.Stat.MOVE_SPEED_V] = 0f;
            }
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
        while (shotCount++ < 3)
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
            if (!target.isAlive)
            {
                if(attackOneCoroutine != null)
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
