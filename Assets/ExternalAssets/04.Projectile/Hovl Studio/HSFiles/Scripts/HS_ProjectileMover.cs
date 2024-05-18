using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class HS_ProjectileMover : MonoBehaviour
{
    [SerializeField] protected float speed = 15f;
    [SerializeField] protected float hitOffset = 0f;
    [SerializeField] protected bool UseFirePointRotation;
    [SerializeField] protected Vector3 rotationOffset = new Vector3(0, 0, 0);
    [SerializeField] protected GameObject hit;
    [SerializeField] protected ParticleSystem hitPS;
    [SerializeField] protected GameObject flash;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected Collider col;
    [SerializeField] protected Light lightSourse;
    [SerializeField] protected GameObject[] Detached;
    [SerializeField] protected ParticleSystem projectilePS;
    private bool startChecker = false;
    [SerializeField]protected bool notDestroy = false;

    private PlayerShooter playerShooter;
    private Vector3 startPosition = Vector3.zero;


    protected virtual void OnEnable()
    {
        playerShooter = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerShooter>();

        if (!startChecker)
        {
            if (flash != null)
            {
                flash.transform.parent = null;
            }
            if (lightSourse != null)
                lightSourse.enabled = true;
            col.enabled = true;
            rb.constraints = RigidbodyConstraints.None;
        }
        startPosition = rb.position = playerShooter.muzzle.transform.position;
        rb.velocity = Vector3.forward * speed;
    }

    protected virtual void FixedUpdate()
    {
        float distance = Vector3.Distance(startPosition, rb.position);

        if (distance >= 20)
        {
            gameObject.SetActive(false);

            playerShooter.ReturnProjectile(gameObject);
        }
    }

}