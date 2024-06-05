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
                selectedCharacterIndex = ParamManager.selectedCharacterIndex;
            }
            return selectedCharacterIndex;
        }
    }

    private void OnEnable()
    {
        for (int i = 0; i < bodyPrefabs.Count; ++i)
        {
            bodyPrefabs[i].SetActive(false);
            headPrefabs[i].SetActive(false);
        }

        bodyPrefabs[SelectedCharacterIndex].SetActive(true);
        headPrefabs[SelectedCharacterIndex].SetActive(true);
    }
}