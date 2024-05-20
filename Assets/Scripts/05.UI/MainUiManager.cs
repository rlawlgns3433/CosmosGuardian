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

        LoadScene("Game");

        //SceneManager.LoadScene(1);
    }

    public void EnterShop()
    {
        LoadScene("Shop");

        //SceneManager.LoadScene(2);
    }

    public void LoadScene(string sceneName)
    {
        PlayerPrefs.SetString("SceneToLoad", sceneName); // 로드할 씬 이름 저장
        SceneManager.LoadScene("Loading"); // 로딩 씬 로드
    }
}
