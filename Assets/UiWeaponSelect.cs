using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiWeaponSelect : MonoBehaviour
{
    public UiCharacterSelect uiCharacterSelect;
    public GameObject[] weapons;
    public int selectedWeaponIndex = 0;

    private void Start()
    {
        selectedWeaponIndex = uiCharacterSelect.selectedWeaponIndex;
        for (int i = 0; i < weapons.Length; ++i)
        {
            if (selectedWeaponIndex == i)
            {
                weapons[i].SetActive(true);
                break;
            }
            else
            {
                weapons[i].SetActive(false);
            }
        }
    }
}
