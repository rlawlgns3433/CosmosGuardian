using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public event Action onDeath;
    public PlayerStats playerStats = null;
    public float maxHealth;
    public float currentHealth;
    private Animator animator;
    private bool isAlive = true;

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
        transform.position = new Vector3(0f, 1f, 0f);   
    }

    private void Start()
    {
        maxHealth = playerStats.stats[CharacterColumn.Stat.Hp];
        currentHealth = maxHealth;

        onDeath += OnDie;
        onDeath += GameManager.Instance.Gameover;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            OnDamage(20);
        }
    }

    public void OnDamage(float damage, Vector3 hitPoint = default, Vector3 hitNormal = default)
    {
        if (!isAlive) return;

        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            onDeath();
        }
    }

    public void OnDie()
    {
        animator.SetTrigger("Die");
    }
}
