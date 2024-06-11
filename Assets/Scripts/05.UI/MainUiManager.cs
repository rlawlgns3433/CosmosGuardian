using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUiManager : MonoBehaviour
{
    public readonly string nameFormat = "{0}Name";
    public UiCharacterSelect uiCharacterSelect;
    private WeaponTable weaponTable;

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
        ParamManager.SceneToLoad = sceneName; // �ε��� �� �̸� ����
        SceneManager.LoadScene((int)SceneIds.Loading); // �ε� �� �ε�
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
