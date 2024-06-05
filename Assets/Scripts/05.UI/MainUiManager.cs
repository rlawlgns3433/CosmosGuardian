using UnityEditor;
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

        ParamManager.SelectedCharacterIndex = uiCharacterSelect.selectedCharacterIndex;
        PlayerPrefs.SetInt("SelectedCharacterIndex", ParamManager.SelectedCharacterIndex);

        LoadScene(SceneIds.Game);
    }

    public void LoadScene(SceneIds sceneName)
    {
        ParamManager.SceneToLoad = sceneName; // 로드할 씬 이름 저장
        SceneManager.LoadScene((int)SceneIds.Loading); // 로딩 씬 로드
    }

    public void QuitApplication()
    {
        PlayerPrefs.SetInt("SelectedCharacterIndex", ParamManager.SelectedCharacterIndex);

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
    }
}
