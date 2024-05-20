using TMPro;
using UnityEngine;

public class UiController : MonoBehaviour
{
    private static readonly string statFormat = "HP : {0}\n데미지 : {1}\n공격빈도 : {2}회/min\n투사체 속도 : {3}\n사거리 : {4}\n크리티컬 확률 : {5}%\n크리티컬 데미지 : {6}%\n관통 : {7}개\n스플래시 범위 : {8}\n스플래시 데미지 : {9}%\n좌우 속도 : {10}%\n전진 속도: {11}%\n체력 흡수 : {12}%\n투사체 개수 : {13}개\n";

    [Header("Group")]
    public GameObject group;
    public GameObject pause;
    public GameObject item;
    public TextMeshProUGUI textStats;
    public PlayerStats playerStats;
    public PlayerShooter playerShooter;

    private void Start()
    {
        group.SetActive(true);
        pause.SetActive(false);
        item.SetActive(false);

        Invoke("UpdatePauseUI", 0.01f);
    }

    public void UpdatePauseUI()
    {
        textStats.text = string.Format(statFormat,
        (int)(playerStats.stats[CharacterColumn.Stat.HP]),
        (int)(playerShooter.weapon.stats[WeaponColumn.Stat.DAMAGE] * playerStats.stats[CharacterColumn.Stat.DAMAGE]),
        (int)(playerShooter.weapon.stats[WeaponColumn.Stat.FIRE_RATE] * playerStats.stats[CharacterColumn.Stat.FIRE_RATE]),
        (int)(playerShooter.weapon.stats[WeaponColumn.Stat.PROJECTILE_SPEED] * playerStats.stats[CharacterColumn.Stat.PROJECTILE_SPEED]),
        (int)(playerShooter.weapon.stats[WeaponColumn.Stat.FIRE_RANGE] * playerStats.stats[CharacterColumn.Stat.FIRE_RANGE]),
        (int)(playerShooter.weapon.stats[WeaponColumn.Stat.CRITICAL] * playerStats.stats[CharacterColumn.Stat.CRITICAL]),
        (int)(playerShooter.weapon.stats[WeaponColumn.Stat.CRITICAL_DAMAGE] * playerStats.stats[CharacterColumn.Stat.CRITICAL_DAMAGE]),
        (int)(playerShooter.weapon.stats[WeaponColumn.Stat.PENETRATE] * playerStats.stats[CharacterColumn.Stat.PENETRATE]),
        (int)(playerShooter.weapon.stats[WeaponColumn.Stat.SPLASH_RANGE] * playerStats.stats[CharacterColumn.Stat.SPLASH_RANGE]),
        (int)(playerShooter.weapon.stats[WeaponColumn.Stat.SPLASH_DAMAGE] * playerStats.stats[CharacterColumn.Stat.SPLASH_DAMAGE]),
        playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_H] * 100f,
        playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_V] * 100f,
        (int)(playerShooter.weapon.stats[WeaponColumn.Stat.HP_DRAIN] * playerStats.stats[CharacterColumn.Stat.HP_DRAIN]),
        (int)(playerShooter.weapon.stats[WeaponColumn.Stat.PROJECTILE_AMOUNT] * playerStats.stats[CharacterColumn.Stat.PROJECTILE_AMOUNT]));
    }
}
