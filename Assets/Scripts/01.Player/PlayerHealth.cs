using System;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public CameraShake cameraShake;
    public TextMeshProUGUI textHealth;
    public event Action onDeath;
    public PlayerStats playerStats = null;
    private Animator animator;
    public bool isAlive = true;
    public Coroutine camShakeCoroutine;

    private void Awake()
    {
        if (!TryGetComponent(out animator))
        {
            animator.enabled = false;
            return;
        }
    }

    //private void OnEnable()
    //{
    //    transform.position = new Vector3(0f, 1f, 0f);
    //}

    private void Start()
    {
        textHealth.text = playerStats.stats[CharacterColumn.Stat.HP].ToString();

        onDeath += OnDie;
        onDeath += GameManager.Instance.Gameover;
    }

    public void OnDamage(float damage, Vector3 hitPoint = default, Vector3 hitNormal = default)
    {
        if (!isAlive) return;

        damage = damage * (1 - (playerStats.stats[CharacterColumn.Stat.ARMOR] - 1));

        playerStats.stats[CharacterColumn.Stat.HP] -= damage;
        //playerStats.SyncDevStat();

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
        animator.SetTrigger("Die");

        Collider[] colliders = GetComponents<Collider>();
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
        if(other.CompareTag(Tags.Enemy))
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
            OnDamage(enemy.enemyData.HP);
            enemy.OnDamage(enemy.enemyData.HP);
        }
    }

}
