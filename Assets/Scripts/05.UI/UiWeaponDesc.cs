using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class UiWeaponDesc : MonoBehaviour
{
    public UiCharacterSelect uiCharacterSelect;
    public TextMeshProUGUI textWindowHeader;
    public TextMeshProUGUI textWeaponDesc;

    private Dictionary<int, WeaponData> data = new Dictionary<int, WeaponData>();
    private void Awake()
    {
        data = uiCharacterSelect.weaponTable.AllData;
    }

    private void OnEnable()
    {
        if(data == null)
        {
            data = uiCharacterSelect.weaponTable.AllData;
        }

        foreach(var weapon in data)
        {
            if(weapon.Key % 100 == uiCharacterSelect.selectedWeaponIndex)
            {
                textWeaponDesc.text = data[weapon.Key].GetDesc;
                textWindowHeader.text = data[weapon.Key].GetName;
                break;
            }
        }
    }
}
