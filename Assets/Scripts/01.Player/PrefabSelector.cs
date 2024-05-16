using System.Collections.Generic;
using UnityEngine;
using static CharacterColumn;

public class PrefabSelector : MonoBehaviour
{
    public List<GameObject> bodyPrefabs = new List<GameObject>();
    public List<GameObject> headPrefabs = new List<GameObject>();

    public int prefabNumber;

    private void OnEnable()
    {
        int selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0); // �� ��° �Ű������� �⺻��

        prefabNumber = selectedCharacterIndex; // �������� ���� �ʿ�

        for (int i = 0; i < bodyPrefabs.Count; ++i)
        {
            bodyPrefabs[i].SetActive(false);
            headPrefabs[i].SetActive(false);
        }

        bodyPrefabs[selectedCharacterIndex].SetActive(true);
        headPrefabs[selectedCharacterIndex].SetActive(true);
    }
}
