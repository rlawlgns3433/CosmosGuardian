using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUiManager : MonoBehaviour
{
    public UiCharacterSelect uiCharacterSelect;
    WeaponTable weaponTable;
    public void EnterGame()
    {
        weaponTable = DataTableMgr.Get<WeaponTable>(DataTableIds.Weapon);
        foreach (var weapon in weaponTable.AllItems)
        {
            if (uiCharacterSelect.selectedWeaponIndex == weapon.PREFAB_ID)
            {
                PlayerPrefs.SetInt("SelectedWeaponId", weapon.WEAPON_ID);
                break;
            }
        }

        PlayerPrefs.SetInt("SelectedCharacterIndex", uiCharacterSelect.selectedCharacterIndex);
        PlayerPrefs.Save();

        SceneManager.LoadScene(1);
    }

    public void EnterShop()
    {
        SceneManager.LoadScene(2);
    }
}
