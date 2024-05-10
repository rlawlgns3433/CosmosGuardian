using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private CharacterTable characterTable;
    private Dictionary<CharacterColumn.Stat, float> stats = new Dictionary<CharacterColumn.Stat, float>(); // �ɷ�ġ ����, �ɷ�ġ ����
    private PrefabSelector prefabSelector;
    private CharacterData characterData;

    private void Awake()
    {
        characterTable = DataTableMgr.Get<CharacterTable>(DataTableIds.Character);
    }

    private void Start()
    {
        if (!TryGetComponent(out prefabSelector))
        {
            prefabSelector.enabled = false;
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.Append((int)TableIdentifier.Character);
        sb.Append(((int)prefabSelector.prefabNumber).ToString("D2"));
        sb.Append(((int)prefabSelector.prefabNumber).ToString("D2")); // id + body(D2) + head(D2)
        string id = sb.ToString();
        characterData = characterTable.Get(int.Parse(id));

        stats[characterData.STAT_1] = characterData.VALUE_1;
        stats[characterData.STAT_2] = characterData.VALUE_2;
        stats[characterData.STAT_3] = characterData.VALUE_3;
        stats[characterData.STAT_4] = characterData.VALUE_4;


        Debug.Log(stats[characterData.STAT_1]);
        Debug.Log(stats[characterData.STAT_2]);
        Debug.Log(stats[characterData.STAT_3]);
        Debug.Log(stats[characterData.STAT_4]);
    }

    // �Ű����� : ���� ����, ������
    public bool UpdateStats(CharacterColumn.Stat stat, float value)
    {
        // Maximum�� ���� ��츦 ����� bool�� ��ȯ

        stats[stat] += value;

        return true;
    }
}
