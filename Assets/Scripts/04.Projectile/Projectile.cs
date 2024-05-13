using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private PlayerShooter playerShooter;
    private PlayerStats playerStats;
    private float range;
    public float Range
    {
        get
        {
            return range * rangeCoefficient;
        }
    }
    public float rangeCoefficient = 20;
    public float speed;
    public float criticalRate;
    public float damage;
    public float splashDamageRate;
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
        // Projectile Table ���� �ʿ�
        range = playerStats.stats[CharacterColumn.Stat.FIRE_RANGE];
        speed = 30.0f;
        criticalRate = 0.3f;
        damage = playerStats.stats[CharacterColumn.Stat.DAMAGE_TYPE_1];

        splashDamageRate = 2.0f;
        splashDamageRange = 5.0f;
        penetrationCount = 1;

        startPosition = playerShooter.muzzle.transform.position;
        transform.position = startPosition;

        rigidbody.velocity = Vector3.zero;  // Rigidbody �ӵ� �ʱ�ȭ
        rigidbody.angularVelocity = Vector3.zero; // ���ӵ� �ʱ�ȭ
    }

    private void Update()
    {
        rigidbody.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);

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
            enemy.OnDamage(damage);
        }
    }
}
