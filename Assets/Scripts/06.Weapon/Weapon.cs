using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject[] weapons;
    public WeaponData weaponData;
    public Dictionary<WeaponColumn.Stat, float> stats = new Dictionary<WeaponColumn.Stat, float>();
    public RuntimeAnimatorController[] animatorControllers;

    private Animator animator;
    private WeaponTable weaponTable;

    private void Awake()
    {
        if(!TryGetComponent(out animator))
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

        stats[WeaponColumn.Stat.DAMAGE] = weaponData.DAMAGE;
        stats[WeaponColumn.Stat.FIRE_RATE] = weaponData.FIRE_RATE;
        stats[WeaponColumn.Stat.FIRE_RANGE] = weaponData.FIRE_RANGE;
        stats[WeaponColumn.Stat.PENETRATE] = weaponData.PENETRATE;
        stats[WeaponColumn.Stat.SPLASH_DAMAGE] = weaponData.SPLASH_DAMAGE;
        stats[WeaponColumn.Stat.SPLASH_RANGE] = weaponData.SPLASH_RANGE;
        stats[WeaponColumn.Stat.CRITICAL] = weaponData.CRITICAL;
        stats[WeaponColumn.Stat.CRITICAL_DAMAGE] = weaponData.CRITICAL_DAMAGE;
        stats[WeaponColumn.Stat.HP_DRAIN] = weaponData.HP_DRAIN;
        stats[WeaponColumn.Stat.PROJECTILE_SPEED] = weaponData.PROJECTILE_SPEED;
        stats[WeaponColumn.Stat.PROJECTILE_AMOUNT] = weaponData.PROJECTILE_AMOUNT;
    }
}
