using System.Text;
using TMPro;
using UnityEngine;

public class UiController : MonoBehaviour
{
    private static readonly string statFormat = "{0} : {1}\n{2} : {3}È¸/min\n{4} : {5}\n{6} : {7}°³\n{8} : {9}\n{10} : {11}\n{12} : {13}%\n{14} : {15}%\n{16} : {17}%\n{18} : {19}%\n{20} : {21}°³\n{22} : {23}\n{24} : {25}%\n{26} : {27}%\n{28} : {29}%\n";
    private static readonly string nameFormat = "{0}Name";

    [Header("Group")]
    public GameObject group;
    public GameObject pause;
    public GameObject item;
    public GameObject stat;
    public GameObject gameover;
    public TextMeshProUGUI textStats;
    public TextMeshProUGUI textItemSelectStats;
    public PlayerStats playerStats;
    public PlayerShooter playerShooter;
    private StringTable stringTable;

    private void Awake()
    {
        stringTable = DataTableMgr.GetStringTable();
    }

    private void Start()
    {
        group.SetActive(true);
        pause.SetActive(false);
        item.SetActive(false);
        stat.SetActive(false);
        gameover.SetActive(false);

        StringBuilder sb = new StringBuilder();

        Invoke("UpdatePauseUI", 0.01f);
    }

    public void UpdatePauseUI()
    {
        int i = 0;

        textItemSelectStats.text = textStats.text = string.Format(statFormat,
        stringTable.Get(string.Format(nameFormat, i++)),
        (int)((playerShooter.weapon.stats[WeaponColumn.Stat.DAMAGE] * playerStats.stats[CharacterColumn.Stat.DAMAGE]) / (playerShooter.weapon.stats[WeaponColumn.Stat.PROJECTILE_AMOUNT] * playerStats.stats[CharacterColumn.Stat.PROJECTILE_AMOUNT])),
        stringTable.Get(string.Format(nameFormat, i++)),
        (int)(playerShooter.weapon.stats[WeaponColumn.Stat.FIRE_RATE] * playerStats.stats[CharacterColumn.Stat.FIRE_RATE]),
        stringTable.Get(string.Format(nameFormat, i++)),
        (int)(playerShooter.weapon.stats[WeaponColumn.Stat.FIRE_RANGE] * playerStats.stats[CharacterColumn.Stat.FIRE_RANGE]),
        stringTable.Get(string.Format(nameFormat, i++)),
        (int)(playerShooter.weapon.stats[WeaponColumn.Stat.PENETRATE] * playerStats.stats[CharacterColumn.Stat.PENETRATE]),
        stringTable.Get(string.Format(nameFormat, i++)),
        (int)(playerShooter.weapon.stats[WeaponColumn.Stat.SPLASH_DAMAGE] * playerStats.stats[CharacterColumn.Stat.SPLASH_DAMAGE]),
        stringTable.Get(string.Format(nameFormat, i++)),
        (playerShooter.weapon.stats[WeaponColumn.Stat.SPLASH_RANGE] * playerStats.stats[CharacterColumn.Stat.SPLASH_RANGE]),
        stringTable.Get(string.Format(nameFormat, i++)),
        (playerShooter.weapon.stats[WeaponColumn.Stat.CRITICAL] * playerStats.stats[CharacterColumn.Stat.CRITICAL]),
        stringTable.Get(string.Format(nameFormat, i++)),
        (int)(playerShooter.weapon.stats[WeaponColumn.Stat.CRITICAL_DAMAGE] * playerStats.stats[CharacterColumn.Stat.CRITICAL_DAMAGE]),
        stringTable.Get(string.Format(nameFormat, i++)),
        (playerShooter.weapon.stats[WeaponColumn.Stat.HP_DRAIN] * playerStats.stats[CharacterColumn.Stat.HP_DRAIN]),
        stringTable.Get(string.Format(nameFormat, i++)),
        (playerShooter.weapon.stats[WeaponColumn.Stat.PROJECTILE_SPEED] * playerStats.stats[CharacterColumn.Stat.PROJECTILE_SPEED]) / (playerStats.initialStats[CharacterColumn.Stat.PROJECTILE_SPEED] * playerShooter.weapon.stats[WeaponColumn.Stat.PROJECTILE_SPEED]) * 100,
        stringTable.Get(string.Format(nameFormat, i++)),
        (int)(playerShooter.weapon.stats[WeaponColumn.Stat.PROJECTILE_AMOUNT] * playerStats.stats[CharacterColumn.Stat.PROJECTILE_AMOUNT]),
        stringTable.Get(string.Format(nameFormat, i++)),
        (int)(playerStats.stats[CharacterColumn.Stat.HP]),
        stringTable.Get(string.Format(nameFormat, i++)),
        playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_V] * 100f,
        stringTable.Get(string.Format(nameFormat, i++)),
        playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_H] * 100f,
        stringTable.Get(string.Format(nameFormat, i++)),
        (playerStats.stats[CharacterColumn.Stat.ARMOR] * 100f) - 100);
    }
}
