using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

// 스테이지가 진행될수록 강력해진다.

/// <summary>
/// 스코어
/// 재화
/// 경험치
/// HP
/// 이동속도
/// </summary>

public class Enemy : MonoBehaviour, IDamageable
{
    public TextMeshProUGUI textHealth;
    public int score;
    public EnemyTable enemyTable;
    public event Action onDeath;
    public float maxHealth;
    public float currentHealth;
    private Animator animator;
    public float damage = 10;
    private PlayerHealth target = null;
    private Coroutine chaseCoroutine = null;
    private WaitForSeconds chaseTimer = new WaitForSeconds(1f);
    private bool isAlive = true;
    public float speed = 1;
    public float rotationSpeed = 180;
    public Vector3 direction;

    private void Awake()
    {
        if(!TryGetComponent(out animator))
        {
            animator.enabled = false;
            return;
        }
    }

    private void Start()
    {
        onDeath += OnDie;
        onDeath += () => { target.GetComponent<PlayerStats>().GetExp(score); };

        direction = (target.transform.position - transform.position).normalized;
    }

    private void OnEnable()
    {
        enemyTable = DataTableMgr.Get<EnemyTable>(DataTableIds.Enemy);
        score = enemyTable.Get(40000).SCORE;

        isAlive = true;
        maxHealth = enemyTable.Get(40000).HP;
        currentHealth = maxHealth;

        Collider[] colliders = gameObject.GetComponents<Collider>();
        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }

        target = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerHealth>();
        isAlive = true;

        chaseCoroutine = StartCoroutine(CoChasePlayer());
    }

    private void OnDisable()
    {
        StopCoroutine(chaseCoroutine);
    }

    private void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
        Vector3 directionToTarget = target.transform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }

    public void OnDamage(float damage, Vector3 hitPoint = default, Vector3 hitNormal = default)
    {
        if (!isAlive) return;

        currentHealth -= damage;
        textHealth.text = currentHealth.ToString();
        if (currentHealth <= 0)
        {
            onDeath();
        }
    }

    public void OnDie()
    {
        isAlive = false;
        StopCoroutine(chaseCoroutine);

        Collider[] colliders = gameObject.GetComponents<Collider>();
        foreach(var collider in colliders)
        {
            collider.enabled = false;
        }

        animator.SetTrigger("Die");
        Destroy(gameObject, 3f);
        speed = 0;
    }

    IEnumerator CoChasePlayer()
    {
        while(isAlive && target.isAlive)
        {
            yield return chaseTimer;

            direction = (target.transform.position - transform.position).normalized;
        }

        if(!target.isAlive)
        {
            direction = Vector3.zero;
        }
    }
}
