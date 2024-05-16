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
        int selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0); // 두 번째 매개변수는 기본값

        prefabNumber = selectedCharacterIndex; // 선택으로 변경 필요

        for (int i = 0; i < bodyPrefabs.Count; ++i)
        {
            bodyPrefabs[i].SetActive(false);
            headPrefabs[i].SetActive(false);
        }

        bodyPrefabs[selectedCharacterIndex].SetActive(true);
        headPrefabs[selectedCharacterIndex].SetActive(true);
    }
}
