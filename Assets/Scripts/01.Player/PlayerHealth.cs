using System;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public CameraShake cameraShake;
    public TextMeshProUGUI textHealth;
    public PlayerStats playerStats;
    public Animator animator;
    public Coroutine camShakeCoroutine;
    public Collider[] colliders;
    public event Action onDeath;
    public bool isAlive = true;

    private void Start()
    {
        textHealth.text = playerStats.stats[CharacterColumn.Stat.HP].ToString();

        onDeath += OnDie;
        onDeath += GameManager.Instance.Gameover;
    }

    public void OnDamage(float damage, bool isCritical = false, Vector3 hitPoint = default, Vector3 hitNormal = default)
    {
        if (!isAlive) return;

        damage = damage * (1 - (playerStats.stats[CharacterColumn.Stat.ARMOR] - 1));

        playerStats.stats[CharacterColumn.Stat.HP] -= damage;

        if (playerStats.stats[CharacterColumn.Stat.HP] <= 0)
        {
            playerStats.stats[CharacterColumn.Stat.HP] = 0;
            UpdateHealthUI();
            onDeath();
        }

        UpdateHealthUI();
    }

    public void OnDie()
    {
        isAlive = false;
        textHealth.gameObject.SetActive(false);
        animator.SetTrigger(Animator.StringToHash("Die"));

        foreach(var collider in colliders)
        {
            collider.enabled = false;
        }
    }
    public void UpdateHealthUI()
    {
        if(isAlive)
        {
            textHealth.text = ((int)playerStats.stats[CharacterColumn.Stat.HP]).ToString();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tags.Enemy) || other.CompareTag(Tags.Elite))
        {
            if(camShakeCoroutine != null)
            {
                StopCoroutine(camShakeCoroutine);
            }

            if(ParamManager.IsCameraShaking)
            {
                camShakeCoroutine = StartCoroutine(cameraShake.Shake(0.15f, 1f));
            }

            var enemy = other.GetComponent<Enemy>();
            float enemyHp = enemy.enemyData.HP;
            float playerHp = playerStats.stats[CharacterColumn.Stat.HP];

            OnDamage(enemyHp);
            enemy.OnDamage(Mathf.Min(playerHp, enemyHp));
        }
    }

}
