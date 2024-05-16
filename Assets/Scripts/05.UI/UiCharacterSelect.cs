using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class UiCharacterSelect : MonoBehaviour
{
    private Animator animator;

    public RuntimeAnimatorController[] animatorControllers;
    public GameObject[] characterBodys;
    public GameObject[] characterHeads;
    public GameObject[] weapons;
    // weapon 선택에 의해 animator가 변경
    public int selectedCharacterIndex = 0;
    public int selectedWeaponIndex = 0;
    private float rotationSpeed = 30;
    private Vector3 rot = new Vector3(0, 1, 0);

    private void Awake()
    {
        if(!TryGetComponent(out animator))
        {
            animator.enabled = false;
        }
    }

    private void Start()
    {
        UpdateCharacter(selectedCharacterIndex);
        UpdateWeapon(selectedWeaponIndex);
    }

    private void Update()
    {
        transform.Rotate(rot * rotationSpeed * Time.deltaTime);
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
        if(weaponIndex == 0)
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

        if (selectedCharacterIndex >= characterBodys.Length )
        {
            selectedCharacterIndex = 0;
        }

        characterBodys[selectedCharacterIndex].SetActive(true);
        characterBodys[selectedCharacterIndex].SetActive(true);

        UpdateCharacter(selectedCharacterIndex);
    }
}