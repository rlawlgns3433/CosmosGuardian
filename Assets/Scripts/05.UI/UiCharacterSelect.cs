using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class UiCharacterSelect : MonoBehaviour
{
    private static readonly string redText = "<color=red>";
    private static readonly string planeText = "<color=white>";
    private static readonly string greenText = "<color=green>";
    private static readonly string colorEndText = "</color>";

    public TextMeshProUGUI textCharacterName;
    public TextMeshProUGUI[] textStats;

    public CharacterStat standardStats = new CharacterStat();
    public CharacterStat charStats = new CharacterStat();
    
    public CharacterTable characterTable;
    public WeaponTable weaponTable;
    public StringTable stringTable;
    public RuntimeAnimatorController[] animatorControllers;
    public GameObject[] characterBodys;
    public GameObject[] characterHeads;
    public GameObject[] weapons;
    public int selectedCharacterIndex = 0;
    public int selectedWeaponIndex = 11;
    private string[] statStringName;
    public Animator animator;


    private void Awake()
    {
        characterTable = DataTableMgr.Get<CharacterTable>(DataTableIds.Character);
        weaponTable = DataTableMgr.Get<WeaponTable>(DataTableIds.Weapon);
        stringTable = DataTableMgr.GetStringTable();

        statStringName = new string[15];
        for (int i = 0; i < 15; ++i)
        {
            statStringName[i] = new string(stringTable.Get(i + "Name"));
        }
    }

    private void Start()
    {
        standardStats.Assign(20101);

        UpdateCharacter(ParamManager.SelectedCharacterIndex);
        UpdateWeapon(ParamManager.selectedWeaponIndex);
    }

    public void UpdateCharacter(int characterIndex)
    {
        selectedCharacterIndex = characterIndex;
        ParamManager.SelectedCharacterIndex = selectedCharacterIndex;

        for (int i = 0; i < characterBodys.Length; ++i)
        {
            if (selectedCharacterIndex == i)
            {
                characterBodys[i].SetActive(true);
                characterHeads[i].SetActive(true);
                break;
            }
            else
            {
                characterBodys[i].SetActive(false);
                characterHeads[i].SetActive(false);
            }
        }

        StringBuilder sb = new StringBuilder();
        sb.Append((int)TableIdentifier.Character);
        sb.Append((selectedCharacterIndex + 1).ToString("D2"));
        sb.Append((selectedCharacterIndex + 1).ToString("D2"));
        string id = sb.ToString();

        int intId = int.Parse(id);
        UpdateStatUi(intId);
        textCharacterName.text = characterTable.Get(intId).GetName;
    }

    public void UpdateWeapon(int weaponIndex)
    {
        if (weaponIndex == 0)
        {
            weapons[weapons.Length - 1].SetActive(false);
        }

        selectedWeaponIndex = weaponIndex;

        for (int i = 0; i < weapons.Length; ++i)
        {
            if (selectedWeaponIndex == i)
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
    }

    public void PrevCharacter()
    {
        characterBodys[selectedCharacterIndex].SetActive(false);
        characterHeads[selectedCharacterIndex].SetActive(false);
        --selectedCharacterIndex;

        if (selectedCharacterIndex < 0)
        {
            selectedCharacterIndex = characterBodys.Length - 1;
        }

        characterBodys[selectedCharacterIndex].SetActive(true);
        characterBodys[selectedCharacterIndex].SetActive(true);

        UpdateCharacter(selectedCharacterIndex);
    }

    public void NextCharacter()
    {
        characterBodys[selectedCharacterIndex].SetActive(false);
        characterHeads[selectedCharacterIndex].SetActive(false);
        ++selectedCharacterIndex;

        if (selectedCharacterIndex >= characterBodys.Length)
        {
            selectedCharacterIndex = 0;
        }

        characterBodys[selectedCharacterIndex].SetActive(true);
        characterBodys[selectedCharacterIndex].SetActive(true);

        UpdateCharacter(selectedCharacterIndex);
    }

    public void UpdateStatUi(int intId)
    {
        charStats.Assign(intId);

        textStats[0].text = statStringName[11];
        textStats[2].text = statStringName[5];
        textStats[4].text = statStringName[14];
        textStats[6].text = statStringName[4];
        textStats[8].text = statStringName[0];
        textStats[10].text = statStringName[6];
        textStats[12].text = statStringName[1];
        textStats[14].text = statStringName[7];
        textStats[16].text = statStringName[2];
        textStats[18].text = statStringName[8];
        textStats[20].text = statStringName[3];
        textStats[22].text = statStringName[10];
        textStats[24].text = statStringName[12];
        textStats[26].text = statStringName[13];

        textStats[1].text = FormatSignedValue(CharacterColumn.Stat.HP, charStats.uiStat[CharacterColumn.Stat.HP]);
        textStats[3].text = FormatSignedValue(CharacterColumn.Stat.SPLASH_RANGE,charStats.uiStat[CharacterColumn.Stat.SPLASH_RANGE]);
        textStats[5].text = FormatSignedValue(CharacterColumn.Stat.ARMOR, charStats.uiStat[CharacterColumn.Stat.ARMOR]);
        textStats[7].text = FormatSignedValue(CharacterColumn.Stat.SPLASH_RANGE, charStats.uiStat[CharacterColumn.Stat.SPLASH_RANGE]);
        textStats[9].text = FormatSignedValue(CharacterColumn.Stat.DAMAGE, charStats.uiStat[CharacterColumn.Stat.DAMAGE]);
        textStats[11].text = FormatSignedValue(CharacterColumn.Stat.CRITICAL, (charStats.uiStat[CharacterColumn.Stat.CRITICAL]));
        textStats[13].text = FormatSignedValue(CharacterColumn.Stat.FIRE_RATE, (charStats.uiStat[CharacterColumn.Stat.FIRE_RATE]));
        textStats[15].text = FormatSignedValue(CharacterColumn.Stat.CRITICAL_DAMAGE, (charStats.uiStat[CharacterColumn.Stat.CRITICAL_DAMAGE]));
        textStats[17].text = FormatSignedValue(CharacterColumn.Stat.FIRE_RANGE, (charStats.uiStat[CharacterColumn.Stat.FIRE_RANGE]));
        textStats[19].text = FormatSignedValue(CharacterColumn.Stat.HP_DRAIN, (charStats.uiStat[CharacterColumn.Stat.HP_DRAIN]));
        textStats[21].text = FormatSignedValue(CharacterColumn.Stat.PENETRATE, (charStats.uiStat[CharacterColumn.Stat.PENETRATE]));
        textStats[23].text = FormatSignedValue(CharacterColumn.Stat.PROJECTILE_AMOUNT, (charStats.uiStat[CharacterColumn.Stat.PROJECTILE_AMOUNT]));
        textStats[25].text = FormatSignedValue(CharacterColumn.Stat.MOVE_SPEED_V, (charStats.uiStat[CharacterColumn.Stat.MOVE_SPEED_V]));
        textStats[27].text = FormatSignedValue(CharacterColumn.Stat.MOVE_SPEED_H, (charStats.uiStat[CharacterColumn.Stat.MOVE_SPEED_H]));
    }

    public string FormatSignedValue(CharacterColumn.Stat stat, float value)
    {
        StringBuilder sb = new StringBuilder();

        if (standardStats.stat[stat] > value)
        {
            sb.Append(redText);
        }
        else if (standardStats.stat[stat] == value)
        {
            sb.Append(planeText);
        }
        else
        {
            sb.Append(greenText);
        }
        sb.Append(value);

        if(stat != CharacterColumn.Stat.HP)
        {
            sb.Append("%");
        }

        sb.Append(colorEndText);

        return sb.ToString();
    }
}