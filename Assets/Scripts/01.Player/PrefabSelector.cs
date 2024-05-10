using System.Collections.Generic;
using UnityEngine;
using static CharacterColumn;

public class PrefabSelector : MonoBehaviour
{
    public List<GameObject> bodyPrefabs = new List<GameObject>();
    public List<GameObject> headPrefabs = new List<GameObject>();

    public CharacterPrefabNumber prefabNumber;

    private void Awake()
    {
        prefabNumber = CharacterPrefabNumber.Anna; // 선택으로 변경 필요

        for(int i = 0; i < bodyPrefabs.Count; ++i)
        {
            bodyPrefabs[i].SetActive(false);
            headPrefabs[i].SetActive(false);
        }

        bodyPrefabs[(int)prefabNumber].SetActive(true);
        headPrefabs[(int)prefabNumber].SetActive(true);
    }
}
