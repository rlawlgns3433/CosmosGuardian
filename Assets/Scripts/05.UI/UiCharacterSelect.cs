using System.Text;
using TMPro;
using UnityEngine;

public class UiCharacterSelect : MonoBehaviour
{
    public TextMeshProUGUI textCharacterName;
    public TextMeshProUGUI textWeaponName;
    public CharacterTable characterTable;
    public WeaponTable weaponTable;
    public RuntimeAnimatorController[] animatorControllers;
    public GameObject[] characterBodys;
    public GameObject[] characterHeads;
    public GameObject[] weapons;
    public int selectedCharacterIndex = 0;
    public int selectedWeaponIndex = 0;
    public int selectedWeaponType = 0;

    private Vector3 rot = new Vector3(0, 1, 0);
    private Animator animator;

    private void Awake()
    {
        characterTable = DataTableMgr.Get<CharacterTable>(DataTableIds.Character);
        weaponTable = DataTableMgr.Get<WeaponTable>(DataTableIds.Weapon);

        if (!TryGetComponent(out animator))
        {
            animator.enabled = false;
        }

        UpdateCharacter(PlayerPrefs.GetInt("SelectedCharacterIndex", 0));
        UpdateWeapon(PlayerPrefs.GetInt("SelectedWeaponId", 0) % 100);
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
        foreach(var _id in weaponTable.AllItemIds)
        {
            if (_id % 100 == selectedWeaponIndex)
            {
                textWeaponName.text = weaponTable.Get(_id).GetName;
                break;
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
}