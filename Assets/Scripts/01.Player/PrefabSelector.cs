using System.Collections.Generic;
using UnityEngine;

public class PrefabSelector : MonoBehaviour
{
    public List<GameObject> bodyPrefabs = new List<GameObject>();
    public List<GameObject> headPrefabs = new List<GameObject>();

    private int selectedCharacterIndex = -1;
    public int SelectedCharacterIndex
    {
        get
        {
            if (selectedCharacterIndex == -1)
            {
                selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
            }
            return selectedCharacterIndex;
        }
    }


    private void Awake()
    {
    }

    private void OnEnable()
    {
        for (int i = 0; i < bodyPrefabs.Count; ++i)
        {
            bodyPrefabs[i].SetActive(false);
            headPrefabs[i].SetActive(false);
        }
        Debug.Log($"Loaded SelectedCharacterIndex: {SelectedCharacterIndex}");
        Debug.Log($"{gameObject.GetInstanceID()}, {this.GetInstanceID()}");

        bodyPrefabs[SelectedCharacterIndex].SetActive(true);
        headPrefabs[SelectedCharacterIndex].SetActive(true);
    }
}
