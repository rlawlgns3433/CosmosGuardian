using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class UiCharacterSelect : MonoBehaviour
{
    public TextMeshProUGUI textCharacterName;
    //public TextMeshProUGUI textWeaponName;
    public TextMeshProUGUI[] textStats;

    public CharacterTable characterTable;
    public WeaponTable weaponTable;
    public StringTable stringTable;
    public RuntimeAnimatorController[] animatorControllers;
    public GameObject[] characterBodys;
    public GameObject[] characterHeads;
    public GameObject[] weapons;
    public int selectedCharacterIndex = 0;
    public int selectedWeaponIndex = 11;
    //public int selectedWeaponIndex = 0;
    //public int selectedWeaponType = 0;

    private string[] statStringName;

    private Vector3 rot = new Vector3(0, 1, 0);
    private Animator animator;

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

        if (!TryGetComponent(out animator))
        {
            animator.enabled = false;
        }

        //ParamManager.selectedWeaponIndex = PlayerPrefs.GetInt("SelectedWeaponId", 0) % 100;
        ParamManager.selectedWeaponIndex = selectedWeaponIndex;

        UpdateCharacter(ParamManager.selectedCharacterIndex);
        UpdateWeapon(ParamManager.selectedWeaponIndex);
    }

    public void UpdateCharacter(int characterIndex)
    {
        selectedCharacterIndex = characterIndex;
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
                // weapon은 prefab id를 구분자로 사용해야 한다.
                // prefab id = index + 1
                // index = prefab id - 1
                // 
                // id + type(D2) + prefabId(D2)
                break;
            }
            else
            {
                weapons[i].SetActive(false);
            }
        }
        //foreach (var _id in weaponTable.AllItemIds)
        //{
        //    if (_id % 100 == selectedWeaponIndex)
        //    {
        //        textWeaponName.text = weaponTable.Get(_id).GetName;
        //        break;
        //    }
        //}
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

        textStats[1].text = characterTable.Get(intId).HP.ToString();
        textStats[3].text = characterTable.Get(intId).SPLASH_RANGE.ToString();
        textStats[5].text = characterTable.Get(intId).ARMOR.ToString();
        textStats[7].text = characterTable.Get(intId).SPLASH_RANGE.ToString();
        textStats[9].text = characterTable.Get(intId).DAMAGE_TYPE_1.ToString();
        textStats[11].text = characterTable.Get(intId).CRITICAL.ToString();
        textStats[13].text = characterTable.Get(intId).FIRE_RATE.ToString();
        textStats[15].text = characterTable.Get(intId).CRITICAL_DAMAGE.ToString();
        textStats[17].text = characterTable.Get(intId).FIRE_RANGE.ToString();
        textStats[19].text = characterTable.Get(intId).HP_DRAIN.ToString();
        textStats[21].text = characterTable.Get(intId).PENETRATE.ToString();
        textStats[23].text = characterTable.Get(intId).PROJECTILE_AMOUNT.ToString();
        textStats[25].text = characterTable.Get(intId).MOVE_SPEED_V.ToString();
        textStats[27].text = characterTable.Get(intId).MOVE_SPEED_H.ToString();
    }
}