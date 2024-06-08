using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Enemy
{
    public static bool IsBossDead = false;
    private int attack1 = Animator.StringToHash("Attack1");
    private int attack2 = Animator.StringToHash("Attack2");
    private int attack3 = Animator.StringToHash("Attack3");

    private List<GameObject> projectiles = new List<GameObject>();

    [Tooltip("Attack 1 발사 간격")]
    public float intervalFloat = 0.3f;
    [Tooltip("Attack 1 발사 속도")]
    public float projectileSpeed = -10;
    [Tooltip("이 거리에 내에 들어올 때 공격")]
    public GameObject projectilePrefab;
    public Coroutine attackOneCoroutine;
    private Platform bossPlatform;
    private CameraMove cameraMove;
    private WaitForSeconds shotInterval;
    public float attackDistance = 30;
    public float angle = 30f;
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

            foreach(var projectile in projectiles)
            {
                Destroy(projectile);
            }

            projectiles.Clear();

            var uiController = GameObject.FindWithTag(Tags.UiController).GetComponent<UiController>();
            uiController.item.SetActive(true);

            StopAllCoroutines();
            cameraMove.IsTOP = true;
            target.playerStats.stats.stat[CharacterColumn.Stat.MOVE_SPEED_V] = savedVerticalSpeed;

            itemController = GameObject.FindWithTag(Tags.ItemController).GetComponent<ItemController>();
            itemController.UpdateItemData(enemyData.TYPE);
            var joystick = GameObject.FindWithTag(Tags.Joystick).GetComponent<FloatingJoystick>();
            joystick.gameObject.SetActive(false);
            joystick.ForcePointerUp();

            Time.timeScale = 0f;
        };

        shotInterval = new WaitForSeconds(intervalFloat);
        if (!Camera.main.TryGetComponent(out cameraMove))
        {
            cameraMove.enabled = false;
            return;
        }
        savedVerticalSpeed = target.playerStats.stats.stat[CharacterColumn.Stat.MOVE_SPEED_V];
        StartCoroutine(AttackPattern());
    }

    private new void Update()
    {
        if (Vector3.Distance(transform.position, target.transform.position) < 12)
        {
            if (isAlive && cameraMove.IsTOP)
            {
                cameraMove.IsTOP = !cameraMove.IsTOP;
                savedVerticalSpeed = target.playerStats.stats.stat[CharacterColumn.Stat.MOVE_SPEED_V];
                target.playerStats.stats.stat[CharacterColumn.Stat.MOVE_SPEED_V] = 0f;
            }
        }

        if(enemyState == EnemyState.Idle)
        {
            animator.SetBool(attack1, false);
            animator.SetBool(attack2, false);
            animator.SetBool(attack3, false);
        }
    }

    public IEnumerator ShotThreeAngle()
    {
        enemyState = EnemyState.Attack1;
        animator.SetBool(attack1, true);
        animator.SetBool(attack2, false);
        animator.SetBool(attack3, false);

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

        projectiles.Add(projectile);
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
            yield return StartCoroutine(ShotThreeAngle());
            enemyState = EnemyState.Idle;
            yield return new WaitForSeconds(3);
        }
    }
}