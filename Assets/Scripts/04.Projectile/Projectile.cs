using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.ParticleSystemJobs;
using static UnityEngine.ParticleSystem;

public class Projectile : MonoBehaviour
{
    private PlayerShooter playerShooter;
    private PlayerStats playerStats;
    public float range;
    public float Range
    {
        get
        {
            return range * rangeCoefficient;
        }
    }
    public float rangeCoefficient = 20;
    public float speed;
    public float Speed
    {
        get
        {
            return speed * speedCoefficient;
        }
    }
    private float speedCoefficient = 20;
    public float criticalRate;
    private float damage;
    public float Damage
    {
        get
        {
            return damage * damageCoefficient;
        }
    }
    private float damageCoefficient = 5;
    public float splashDamage;
    public float splashDamageRange;
    public int penetrationCount;
    public Vector3 startPosition = Vector3.zero;
    public Rigidbody rigidbody;

    private void OnEnable()
    {
        if (playerShooter == null && !GameObject.FindWithTag(Tags.Player).TryGetComponent(out playerShooter))
        {
            playerShooter.enabled = false;
            return;
        }

        if (playerStats == null && !GameObject.FindWithTag(Tags.Player).TryGetComponent(out playerStats))
        {
            playerStats.enabled = false;
            return;
        }

        //range = playerStats.stats[CharacterColumn.Stat.FIRE_RANGE];
        //speed = playerStats.stats[CharacterColumn.Stat.FIRE_RATE];
        criticalRate = playerStats.stats[CharacterColumn.Stat.CRITICAL];
        damage = playerStats.stats[CharacterColumn.Stat.DAMAGE];

        splashDamage = playerStats.stats[CharacterColumn.Stat.SPLASH_DAMAGE];
        splashDamageRange = playerStats.stats[CharacterColumn.Stat.SPLASH_RANGE];
        penetrationCount = (int)playerStats.stats[CharacterColumn.Stat.PENENTRATE];

        startPosition = playerShooter.muzzle.transform.position;
        transform.position = startPosition;

        rigidbody.velocity = Vector3.zero;  // Rigidbody 속도 초기화
        rigidbody.angularVelocity = Vector3.zero; // 각속도 초기화
    }

    private void Update()
    {
        rigidbody.AddForce(transform.forward * Speed * Time.deltaTime, ForceMode.Impulse);

        float distance = Vector3.Distance(startPosition, transform.position);

        if (distance >= Range)
        {
            gameObject.SetActive(false);
            playerShooter.ReturnProjectile(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tags.Enemy))
        {
            var enemy = other.gameObject.GetComponent<Enemy>();
            enemy.OnDamage(Damage);

            gameObject.SetActive(false);
            playerShooter.ReturnProjectile(gameObject);
        }
    }
}
