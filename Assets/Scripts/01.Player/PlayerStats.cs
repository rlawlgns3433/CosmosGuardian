using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private CharacterTable characterTable = null;
    private Dictionary<CharacterColumn.Stat, float> stats = new Dictionary<CharacterColumn.Stat, float>(); // �ɷ�ġ ����, �ɷ�ġ ����
    private PrefabSelector prefabSelector = null;
    public CharacterData characterData = null;

    // �� �κ��� Scriptable Assets�� ������ ���� ����
    public int price = default;
    public float maxHp = default;
    public float armor = default;
    public float damageType1 = default;
    public float damageType2 = default;
    public float damageType3 = default;
    public float speedVertical = default;
    public float speedHorizontal = default;

    private void Awake()
    {
        characterTable = DataTableMgr.Get<CharacterTable>(DataTableIds.Character);
    }

    private void OnEnable()
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

        price = characterData.PRICE;
        maxHp = characterData.HP;
        armor = characterData.ARMOR;
        damageType1 = characterData.DAMAGE_TYPE_1;
        damageType2 = characterData.DAMAGE_TYPE_2;
        damageType3 = characterData.DAMAGE_TYPE_3;
        speedVertical = characterData.MOVE_SPEED_V;
        speedHorizontal = characterData.MOVE_SPEED_H;
    }

    // �Ű����� : ���� ����, ������
    public bool UpdateStats(CharacterColumn.Stat stat, float value)
    {
        // Maximum�� ���� ��츦 ����� bool�� ��ȯ

        stats[stat] += value;

        return true;
    }
}
