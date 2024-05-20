using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject[] weapons;
    public WeaponData weaponData;
    public Dictionary<WeaponColumn.Stat, float> initialStats = new Dictionary<WeaponColumn.Stat, float>();
    public Dictionary<WeaponColumn.Stat, float> stats = new Dictionary<WeaponColumn.Stat, float>();
    public RuntimeAnimatorController[] animatorControllers;

    private Animator animator;
    private WeaponTable weaponTable;

    private void Awake()
    {
        if (!TryGetComponent(out animator))
        {
            animator.enabled = false;
            return;
        }
    }

    private void Start()
    {
        weaponTable = DataTableMgr.Get<WeaponTable>(DataTableIds.Weapon);
        int selectedWeaponId = PlayerPrefs.GetInt("SelectedWeaponId", 0);
        SetWeapon(selectedWeaponId);
    }

    public void SetWeapon(int weaponId)
    {
        weaponData = weaponTable.Get(weaponId);


        for (int i = 0; i < weapons.Length; ++i)
        {
            if (weaponData.PREFAB_ID == i)
            {
                animator.runtimeAnimatorController = animatorControllers[i];
                weapons[i].SetActive(true);
                break;
            }
            else
            {
                weapons[i].SetActive(false);
            }
        }

        initialStats[WeaponColumn.Stat.DAMAGE] = stats[WeaponColumn.Stat.DAMAGE] = weaponData.DAMAGE;
        initialStats[WeaponColumn.Stat.FIRE_RATE] = stats[WeaponColumn.Stat.FIRE_RATE] = weaponData.FIRE_RATE;
        initialStats[WeaponColumn.Stat.FIRE_RANGE] = stats[WeaponColumn.Stat.FIRE_RANGE] = weaponData.FIRE_RANGE;
        initialStats[WeaponColumn.Stat.PENETRATE] = stats[WeaponColumn.Stat.PENETRATE] = weaponData.PENETRATE;
        initialStats[WeaponColumn.Stat.SPLASH_DAMAGE] = stats[WeaponColumn.Stat.SPLASH_DAMAGE] = weaponData.SPLASH_DAMAGE;
        initialStats[WeaponColumn.Stat.SPLASH_RANGE] = stats[WeaponColumn.Stat.SPLASH_RANGE] = weaponData.SPLASH_RANGE;
        initialStats[WeaponColumn.Stat.CRITICAL] = stats[WeaponColumn.Stat.CRITICAL] = weaponData.CRITICAL;
        initialStats[WeaponColumn.Stat.CRITICAL_DAMAGE] = stats[WeaponColumn.Stat.CRITICAL_DAMAGE] = weaponData.CRITICAL_DAMAGE;
        initialStats[WeaponColumn.Stat.HP_DRAIN] = stats[WeaponColumn.Stat.HP_DRAIN] = weaponData.HP_DRAIN;
        initialStats[WeaponColumn.Stat.PROJECTILE_SPEED] = stats[WeaponColumn.Stat.PROJECTILE_SPEED] = weaponData.PROJECTILE_SPEED;
        initialStats[WeaponColumn.Stat.PROJECTILE_AMOUNT] = stats[WeaponColumn.Stat.PROJECTILE_AMOUNT] = weaponData.PROJECTILE_AMOUNT;
    }
}
