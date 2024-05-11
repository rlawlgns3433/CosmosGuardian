using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMove : MonoBehaviour
{
    public PlayerShooter playerShooter;
    public float range;
    public float speed;
    public float criticalRate;
    public float damage;
    public float splashDamageRate;
    public float splashDamageRange;
    public int penetrationCount;
    Vector3 direction = Vector3.forward;
    Vector3 startPosition = Vector3.zero;
    public Rigidbody rigidbody;
    private void OnEnable()
    {
        // Projectile Table 별도 필요
        range = 50.0f;
        speed = 50.0f;
        criticalRate = 0.3f;
        damage = 10f;
        splashDamageRate = 2.0f;
        splashDamageRange = 5.0f;
        penetrationCount = 1;

        rigidbody.velocity = Vector3.zero;  // Rigidbody 속도 초기화
        rigidbody.angularVelocity = Vector3.zero; // 각속도 초기화
    }

    private void FixedUpdate()
    {
        rigidbody.AddForce(transform.forward * speed * Time.deltaTime, ForceMode.Impulse);

        float distance = Vector3.Distance(startPosition, transform.position);

        if (distance >= range)
        {
            gameObject.SetActive(false);

            playerShooter.ReturnProjectile(gameObject);
        }
    }
}
