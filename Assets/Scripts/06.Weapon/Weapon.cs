using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject[] weapons;
    public WeaponData weaponData;
    public Dictionary<WeaponColumn.Stat, float> stats = new Dictionary<WeaponColumn.Stat, float>();
    public RuntimeAnimatorController[] animatorControllers;
    public PlayerStats playerStats;
    private UiController uiController;

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
    }

    private void Start()
    {
        if (!GameObject.FindWithTag(Tags.UiController).TryGetComponent(out uiController))
        {
            uiController.enabled = false;
            return;
        }

        SetWeapon(selectedWeaponId);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetWeapon(30101);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetWeapon(30115);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SetWeapon(30111);
        }
    }


    public void SetWeapon(int weaponId)
    {
        weaponData = weaponTable.Get(weaponId);
        selectedWeaponId = weaponId;

        foreach (var p in playerStats.playerShooter.usingProjectiles)
        {
            Destroy(p);
        }

        foreach (var p in playerStats.playerShooter.unusingProjectiles)
        {
            Destroy(p);
        }

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
