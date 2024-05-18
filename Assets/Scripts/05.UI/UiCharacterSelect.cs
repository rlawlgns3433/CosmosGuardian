using UnityEngine;

public class UiCharacterSelect : MonoBehaviour
{
    public CharacterTable characterTable;
    public WeaponTable weaponTable;
    public RuntimeAnimatorController[] animatorControllers;
    public GameObject[] characterBodys;
    public GameObject[] characterHeads;
    public GameObject[] weapons;
    public int selectedCharacterIndex = 0;
    public int selectedWeaponIndex = 0;

    private Vector3 rot = new Vector3(0, 1, 0);
    private Animator animator;

    private void Awake()
    {
        if (!TryGetComponent(out animator))
        {
            animator.enabled = false;
        }

        UpdateCharacter(PlayerPrefs.GetInt("SelectedCharacterIndex", 0));
        UpdateWeapon(PlayerPrefs.GetInt("SelectedWeaponId", 0) % 100);
    }

    private void Start()
    {
        characterTable = DataTableMgr.Get<CharacterTable>(DataTableIds.Character);
        weaponTable = DataTableMgr.Get<WeaponTable>(DataTableIds.Weapon);
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
}