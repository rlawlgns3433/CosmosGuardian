using Mono.Cecil.Cil;
using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public event Action onDeath;
    public PlayerStats playerStats = null;
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
        onDeath += OnDie;
        onDeath += GameManager.Instance.Gameover;
    }

    public void OnDamage(float damage, Vector3 hitPoint = default, Vector3 hitNormal = default)
    {
        if (!isAlive) return;

        playerStats.stats[CharacterColumn.Stat.HP] -= damage;
        if (playerStats.stats[CharacterColumn.Stat.HP] <= 0)
        {
            onDeath();
        }
    }

    public void OnDie()
    {
        isAlive = false;
        animator.SetTrigger("Die");

        Collider[] colliders = GetComponents<Collider>();
        foreach(var collider in colliders)
        {
            collider.enabled = false;
        }
    }
    public void RestoreHealth(float amount)
    {
        playerStats.stats[CharacterColumn.Stat.HP] += amount;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            var enemy = other.GetComponent<Enemy>();
            OnDamage(enemy.currentHealth);
            enemy.OnDie();
            Debug.Log($"Damaged : {playerStats.stats[CharacterColumn.Stat.HP]}");
        }
    }
}
