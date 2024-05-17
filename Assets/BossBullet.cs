using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public Boss boss;

    private void Start()
    {
        if (!GameObject.FindWithTag("Boss").TryGetComponent(out boss))
        {
            boss.enabled = false;
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tags.Player))
        {
            var player = other.GetComponent<PlayerHealth>();
            player.OnDamage(boss.enemyData.DAMAGE);

            Destroy(gameObject);
        }
    }
}
