using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    WeaponTable weaponTable;
    public GameObject[] weapons;
    public WeaponData weaponData;
    public Dictionary<WeaponColumn.Stat, float> stats = new Dictionary<WeaponColumn.Stat, float>();
    public RuntimeAnimatorController[] animatorControllers;
    private Animator animator;

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
        int selectedWeaponId = PlayerPrefs.GetInt("SelectedWeaponId", 0); // 두 번째 매개변수는 기본값
        SetWeapon(selectedWeaponId); // 무기 선택으로 변경 필요
    }

    public void SetWeapon(int weaponId)
    {

        Debug.Log(weaponId);
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

        /*
        DAMAGE,
        FIRE_RATE,
        FIRE_RANGE,
        PENENTRATE,
        SPLASH_DAMAGE,
        SPLASH_RANGE,
        CRITICAL,
        CRITICAL_DAMAGE,
        HP_DRAIN,
        PROJECTILE_SPEED,
        PROJECTILE_AMOUNT,
         */

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


        Debug.Log($"{stats[WeaponColumn.Stat.DAMAGE]} / {stats[WeaponColumn.Stat.FIRE_RATE]} / {stats[WeaponColumn.Stat.FIRE_RANGE]} / {stats[WeaponColumn.Stat.PENETRATE]} / {stats[WeaponColumn.Stat.SPLASH_DAMAGE]}");
    }
}
