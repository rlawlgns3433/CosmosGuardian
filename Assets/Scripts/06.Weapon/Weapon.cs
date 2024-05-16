using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    WeaponTable weaponTable;

    public WeaponData weaponData;
    public Dictionary<WeaponColumn.Stat, float> stats = new Dictionary<WeaponColumn.Stat, float>();

    private void Start()
    {
        weaponTable = DataTableMgr.Get<WeaponTable>(DataTableIds.Weapon);
        SetWeapon(30103); // ���� �������� ���� �ʿ�
    }

    public void SetWeapon(int weaponId)
    {
        weaponData = weaponTable.Get(weaponId);

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
    }
}