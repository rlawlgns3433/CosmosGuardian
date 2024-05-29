using System.Text;
using TMPro;
using UnityEngine;

public class UiController : MonoBehaviour
{
    private static readonly string nameFormat = "{0}Name";

    public GameObject group;
    public GameObject pause;
    public GameObject item;
    public GameObject stat;
    public GameObject gameover;
    public TextMeshProUGUI textStats;
    public TextMeshProUGUI[] textStatNamesInPause;
    public TextMeshProUGUI[] textStatInPause;
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

        #region LeftSide
        textStatNamesInPause[i].text = stringTable.Get(string.Format(nameFormat, 11));
        textStatInPause[i++].text = ((int)playerStats.stats[CharacterColumn.Stat.HP]).ToString();

        textStatNamesInPause[i].text = stringTable.Get(string.Format(nameFormat, 14));
        textStatInPause[i++].text = ((playerStats.stats[CharacterColumn.Stat.ARMOR] * 100f) - 100 + "%").ToString();

        textStatNamesInPause[i].text = stringTable.Get(string.Format(nameFormat, 0));
        textStatInPause[i++].text = ((int)((playerShooter.weapon.stats[WeaponColumn.Stat.DAMAGE] * playerStats.stats[CharacterColumn.Stat.DAMAGE]) / (playerShooter.weapon.stats[WeaponColumn.Stat.PROJECTILE_AMOUNT] * playerStats.stats[CharacterColumn.Stat.PROJECTILE_AMOUNT]))).ToString();

        textStatNamesInPause[i].text = stringTable.Get(string.Format(nameFormat, 1));
        textStatInPause[i++].text = ((int)(playerShooter.weapon.stats[WeaponColumn.Stat.FIRE_RATE] * playerStats.stats[CharacterColumn.Stat.FIRE_RATE]) + "È¸/ºÐ").ToString();

        textStatNamesInPause[i].text = stringTable.Get(string.Format(nameFormat, 2));
        textStatInPause[i++].text = ((int)(playerShooter.weapon.stats[WeaponColumn.Stat.FIRE_RANGE] * playerStats.stats[CharacterColumn.Stat.FIRE_RANGE]) + "À¯´Ö").ToString();

        textStatNamesInPause[i].text = stringTable.Get(string.Format(nameFormat, 3));
        textStatInPause[i++].text = ((int)(playerShooter.weapon.stats[WeaponColumn.Stat.PENETRATE] * playerStats.stats[CharacterColumn.Stat.PENETRATE]) + "°³").ToString();

        textStatNamesInPause[i].text = stringTable.Get(string.Format(nameFormat, 12));
        textStatInPause[i++].text = (playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_V] * 100f + "%").ToString();
        #endregion

        #region RightSide
        textStatNamesInPause[i].text = stringTable.Get(string.Format(nameFormat, 5));
        textStatInPause[i++].text = (playerShooter.weapon.stats[WeaponColumn.Stat.SPLASH_RANGE] * playerStats.stats[CharacterColumn.Stat.SPLASH_RANGE] + "À¯´Ö").ToString();

        textStatNamesInPause[i].text = stringTable.Get(string.Format(nameFormat, 4));
        textStatInPause[i++].text = ((int)(playerShooter.weapon.stats[WeaponColumn.Stat.SPLASH_DAMAGE] * playerStats.stats[CharacterColumn.Stat.SPLASH_DAMAGE]) + "%").ToString();

        textStatNamesInPause[i].text = stringTable.Get(string.Format(nameFormat, 6));
        textStatInPause[i++].text = (playerShooter.weapon.stats[WeaponColumn.Stat.CRITICAL] * playerStats.stats[CharacterColumn.Stat.CRITICAL] + "%").ToString();

        textStatNamesInPause[i].text = stringTable.Get(string.Format(nameFormat, 7));
        textStatInPause[i++].text = ((int)(playerShooter.weapon.stats[WeaponColumn.Stat.CRITICAL_DAMAGE] * playerStats.stats[CharacterColumn.Stat.CRITICAL_DAMAGE]) + "%").ToString();

        textStatNamesInPause[i].text = stringTable.Get(string.Format(nameFormat, 8));
        textStatInPause[i++].text = ((playerShooter.weapon.stats[WeaponColumn.Stat.HP_DRAIN] * playerStats.stats[CharacterColumn.Stat.HP_DRAIN]) + "%").ToString();

        textStatNamesInPause[i].text = stringTable.Get(string.Format(nameFormat, 10));
        textStatInPause[i++].text = ((int)(playerShooter.weapon.stats[WeaponColumn.Stat.PROJECTILE_AMOUNT] * playerStats.stats[CharacterColumn.Stat.PROJECTILE_AMOUNT]) + "°³").ToString();

        textStatNamesInPause[i].text = stringTable.Get(string.Format(nameFormat, 13));
        textStatInPause[i++].text = (playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_H] * 100f + "%").ToString();
        #endregion
    }
}
