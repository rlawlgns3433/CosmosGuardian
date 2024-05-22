using UnityEngine;

public interface IDamageable
{
    void OnDamage(float damage, bool isCritical, Vector3 hitPoint, Vector3 hitNormal);
    void OnDie();
}