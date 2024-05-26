using UnityEngine;

public class EliteBullet : MonoBehaviour
{
    public float damage;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Tags.Player))
        {
            var player = other.GetComponent<PlayerHealth>();
            player.OnDamage(damage);

            if (player.camShakeCoroutine != null)
            {
                StopCoroutine(player.camShakeCoroutine);
            }

            if (ParamManager.IsCameraShaking)
            {
                player.camShakeCoroutine = StartCoroutine(player.cameraShake.Shake(0.15f, 1f));
            }

            Destroy(gameObject);
        }
    }
}
