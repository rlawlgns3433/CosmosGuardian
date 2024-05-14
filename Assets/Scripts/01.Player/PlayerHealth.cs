using Mono.Cecil.Cil;
using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public event Action onDeath;
    public PlayerStats playerStats = null;
    public float maxHealth;
    public float currentHealth;
    private Animator animator;
    public bool isAlive = true;

    private void Awake()
    {
        if (!TryGetComponent(out animator))
        {
            animator.enabled = false;
            return;
        }
    }

    private void OnEnable()
    {
        transform.position = new Vector3(0f, 1f, 0f);
    }

    private void Start()
    {
        maxHealth = playerStats.stats[CharacterColumn.Stat.HP];
        currentHealth = maxHealth;

        onDeath += OnDie;
        onDeath += GameManager.Instance.Gameover;
    }

    public void OnDamage(float damage, Vector3 hitPoint = default, Vector3 hitNormal = default)
    {
        if (!isAlive) return;

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            onDeath();
        }
    }

    public void OnDie()
    {
        isAlive = false;
        animator.SetTrigger("Die");
    }

    public void UpgradeHealth(float amount)
    {
        maxHealth += amount;
    }

    public void RestoreHealth(float amount)
    {
        currentHealth += amount;

        if (maxHealth < currentHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();
            OnDamage(enemy.currentHealth);
            enemy.OnDie();
            Debug.Log($"Damaged : {currentHealth}");
        }
    }
}
