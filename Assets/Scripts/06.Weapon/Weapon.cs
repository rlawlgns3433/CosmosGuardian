using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject[] weapons;
    private WeaponData _weaponData;
    public WeaponData weaponData
    {
        get
        {
            if(_weaponData == null)
                return weaponTable.Get(selectedWeaponId);
            return _weaponData;
        }
    }
    public Dictionary<WeaponColumn.Stat, float> stats = new Dictionary<WeaponColumn.Stat, float>();
    public RuntimeAnimatorController[] animatorControllers;
    public PlayerStats playerStats;
    public UiController uiController;

    private WeaponTable weaponTable;
    public int selectedWeaponId = default;
    int currentWeaponIndex = -1;

    private void Awake()
    {
        weaponTable = DataTableMgr.Get<WeaponTable>(DataTableIds.Weapon);
        selectedWeaponId = PlayerPrefs.GetInt("SelectedWeaponId", 0);
    }

    private void Start()
    {
        SetWeapon(selectedWeaponId);
    }

    public void SetWeapon(int weaponId)
    {
        _weaponData = weaponTable.Get(weaponId);
        selectedWeaponId = weaponId;
        playerStats.playerShooter.currentProjectileIndex = _weaponData.PROJECTILE_ID - 1;


        if (currentWeaponIndex != -1)
        {
            weapons[currentWeaponIndex].SetActive(false);
        }

        for (int i = 0; i < weapons.Length; ++i)
        {
            if (_weaponData.PREFAB_ID == i)
            {
                playerStats.playerHealth.animator.runtimeAnimatorController = animatorControllers[i];
                weapons[i].SetActive(true);
                currentWeaponIndex = i;
                break;
            }
            else
            {
                weapons[i].SetActive(false);
            }
        }

        stats[WeaponColumn.Stat.DAMAGE] = _weaponData.DAMAGE;
        stats[WeaponColumn.Stat.FIRE_RATE] = _weaponData.FIRE_RATE;
        stats[WeaponColumn.Stat.FIRE_RANGE] = _weaponData.FIRE_RANGE;
        stats[WeaponColumn.Stat.PENETRATE] = _weaponData.PENETRATE;
        stats[WeaponColumn.Stat.SPLASH_DAMAGE] = _weaponData.SPLASH_DAMAGE;
        stats[WeaponColumn.Stat.SPLASH_RANGE] = _weaponData.SPLASH_RANGE;
        stats[WeaponColumn.Stat.CRITICAL] = _weaponData.CRITICAL;
        stats[WeaponColumn.Stat.CRITICAL_DAMAGE] = _weaponData.CRITICAL_DAMAGE;
        stats[WeaponColumn.Stat.HP_DRAIN] = _weaponData.HP_DRAIN;
        stats[WeaponColumn.Stat.PROJECTILE_SPEED] = _weaponData.PROJECTILE_SPEED;
        stats[WeaponColumn.Stat.PROJECTILE_AMOUNT] = _weaponData.PROJECTILE_AMOUNT;

        foreach (var wd in playerStats.playerWeaponDatas)
        {
            playerStats.UpdateStats(wd.playerStat, OptionColumn.Type.ApplyChangeWeaponData, wd.playerValue);
        }

        uiController.UpdatePauseUI();
    }
}