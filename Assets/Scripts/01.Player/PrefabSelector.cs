using System.Collections.Generic;
using UnityEngine;

public class PrefabSelector : MonoBehaviour
{
    public List<GameObject> bodyPrefabs = new List<GameObject>();
    public List<GameObject> headPrefabs = new List<GameObject>();

    public int prefabNumber;
    public int selectedCharacterIndex;

    private void Awake()
    {
        selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
        Debug.Log($"SelectedCharacterIndex : {selectedCharacterIndex}");
    }

    private void OnEnable()
    {
        prefabNumber = selectedCharacterIndex;

        for (int i = 0; i < bodyPrefabs.Count; ++i)
        {
            bodyPrefabs[i].SetActive(false);
            headPrefabs[i].SetActive(false);
        }
        Debug.Log($"Loaded SelectedCharacterIndex: {selectedCharacterIndex}");
        bodyPrefabs[selectedCharacterIndex].SetActive(true);
        headPrefabs[selectedCharacterIndex].SetActive(true);
    }
}
