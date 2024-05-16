using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class UiCharacterSelect : MonoBehaviour
{
    private Animator animator;

    public RuntimeAnimatorController[] animatorControllers;
    public GameObject[] weapons;
    // weapon 선택에 의해 animator가 변경
    public int selectedCharacterIndex = 0;
    public int selectedWeaponIndex = 0;

    private void Awake()
    {
        if(!TryGetComponent(out animator))
        {
            animator.enabled = false;
        }
    }

    private void Start()
    {
        UpdateWeapon(selectedWeaponIndex);
    }

    public void UpdateWeapon(int weaponIndex)
    {

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
}