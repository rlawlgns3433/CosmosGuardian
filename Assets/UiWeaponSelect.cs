using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiWeaponSelect : MonoBehaviour
{
    public UiCharacterSelect uiCharacterSelect;
    public GameObject[] weapons;
    public int selectedWeaponIndex = 0;
    public int weaponCount = 0;

    private void Start()
    {
        weaponCount = weapons.Length;

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

    public void PrevButton()
    {
        weapons[selectedWeaponIndex].SetActive(false);
        --selectedWeaponIndex;

        if(selectedWeaponIndex < 0)
        {
            selectedWeaponIndex = weaponCount - 1;
        }

        weapons[selectedWeaponIndex].SetActive(true);
        uiCharacterSelect.UpdateWeapon(selectedWeaponIndex);
    }

    public void NextWeapon()
    {
        weapons[selectedWeaponIndex].SetActive(false);
        ++selectedWeaponIndex;

        if (selectedWeaponIndex >= weaponCount)
        {
            selectedWeaponIndex = 0;
        }

        weapons[selectedWeaponIndex].SetActive(true);
        uiCharacterSelect.UpdateWeapon(selectedWeaponIndex);
    }
}
