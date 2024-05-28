using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject[] weapons;
    public WeaponData weaponData;
    public Dictionary<WeaponColumn.Stat, float> stats = new Dictionary<WeaponColumn.Stat, float>();
    public RuntimeAnimatorController[] animatorControllers;
    public PlayerStats playerStats;
    public UiController uiController;

    private Animator animator;
    private WeaponTable weaponTable;
    public int selectedWeaponId = default;
    int currentWeaponIndex = -1;

    private void Awake()
    {
        weaponTable = DataTableMgr.Get<WeaponTable>(DataTableIds.Weapon);
        selectedWeaponId = PlayerPrefs.GetInt("SelectedWeaponId", 0);

        if (!TryGetComponent(out animator))
        {
            animator.enabled = false;
            return;
        }
        SetWeapon(selectedWeaponId);
    }

    public void SetWeapon(int weaponId)
    {
        if( weaponData != null && weaponData.PROJECTILE_ID != weaponTable.Get(weaponId).PROJECTILE_ID)
        {
            foreach (var p in playerStats.playerShooter.usingProjectiles)
            {
                Destroy(p);
            }

            foreach (var p in playerStats.playerShooter.unusingProjectiles)
            {
                Destroy(p);
            }
        }

        weaponData = weaponTable.Get(weaponId);
        selectedWeaponId = weaponId;

        playerStats.playerShooter.usingProjectiles.Clear();
        playerStats.playerShooter.unusingProjectiles.Clear();
        playerStats.playerShooter.currentProjectileIndex = weaponData.PROJECTILE_ID - 1;


        if(currentWeaponIndex != -1)
        {
            weapons[currentWeaponIndex].SetActive(false);
        }

        for (int i = 0; i < weapons.Length; ++i)
        {
            if (weaponData.PREFAB_ID == i)
            {
                animator.runtimeAnimatorController = animatorControllers[i];
                weapons[i].SetActive(true);
                currentWeaponIndex = i;
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

        foreach (var wd in playerStats.playerWeaponDatas)
        {
            playerStats.UpdateStats(wd.playerStat, OptionColumn.Type.ApplyChangeWeaponData, wd.playerValue);
        }

        uiController.UpdatePauseUI();
    }
}
