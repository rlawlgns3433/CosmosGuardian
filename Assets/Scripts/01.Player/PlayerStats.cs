using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private CharacterTable characterTable = null;
    private Dictionary<CharacterColumn.Stat, float> initialStats = new Dictionary<CharacterColumn.Stat, float>(); // �ɷ�ġ ����, �ɷ�ġ ����
    public Dictionary<CharacterColumn.Stat, float> stats = new Dictionary<CharacterColumn.Stat, float>(); // �ɷ�ġ ����, �ɷ�ġ ����
    private PrefabSelector prefabSelector = null;
    public CharacterData characterData = null;

    // �� �κ��� Scriptable Assets�� ������ ���� ����
    public int price = default;

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

        stats[CharacterColumn.Stat.HP] = initialStats[CharacterColumn.Stat.HP] = characterData.HP;
        stats[CharacterColumn.Stat.ARMOR] = initialStats[CharacterColumn.Stat.ARMOR] = characterData.ARMOR;
        stats[CharacterColumn.Stat.DAMAGE_TYPE_1] = initialStats[CharacterColumn.Stat.DAMAGE_TYPE_1] = characterData.DAMAGE_TYPE_1;
        stats[CharacterColumn.Stat.DAMAGE_TYPE_2] = initialStats[CharacterColumn.Stat.DAMAGE_TYPE_2] = characterData.DAMAGE_TYPE_2;
        stats[CharacterColumn.Stat.DAMAGE_TYPE_3] = initialStats[CharacterColumn.Stat.DAMAGE_TYPE_3] = characterData.DAMAGE_TYPE_3;
        stats[CharacterColumn.Stat.MOVE_SPEED_V] = initialStats[CharacterColumn.Stat.MOVE_SPEED_V] = characterData.MOVE_SPEED_V;
        stats[CharacterColumn.Stat.MOVE_SPEED_H] = initialStats[CharacterColumn.Stat.MOVE_SPEED_H] = characterData.MOVE_SPEED_H;
        stats[CharacterColumn.Stat.FIRE_RATE] = initialStats[CharacterColumn.Stat.FIRE_RATE] = characterData.FIRE_RATE;
        stats[CharacterColumn.Stat.FIRE_RANGE] = initialStats[CharacterColumn.Stat.FIRE_RANGE] = characterData.FIRE_RANGE;
        stats[CharacterColumn.Stat.PENENTRATE] = initialStats[CharacterColumn.Stat.PENENTRATE] = characterData.PENENTRATE;
        stats[CharacterColumn.Stat.SPLASH_DAMAGE] = initialStats[CharacterColumn.Stat.SPLASH_DAMAGE] = characterData.SPLASH_DAMAGE;
        stats[CharacterColumn.Stat.SPLASH_RANGE] = initialStats[CharacterColumn.Stat.SPLASH_RANGE] = characterData.SPLASH_RANGE;
        stats[CharacterColumn.Stat.CRITICAL] = initialStats[CharacterColumn.Stat.CRITICAL] = characterData.CRITICAL;
        stats[CharacterColumn.Stat.CRITICAL_DAMAGE] = initialStats[CharacterColumn.Stat.CRITICAL_DAMAGE] = characterData.CRITICAL_DAMAGE;
        stats[CharacterColumn.Stat.HP_DRAIN] = initialStats[CharacterColumn.Stat.HP_DRAIN] = characterData.HP_DRAIN;
        stats[CharacterColumn.Stat.PROJECTILE_SPEED] = initialStats[CharacterColumn.Stat.PROJECTILE_SPEED] = characterData.PROJECTILE_SPEED;
        stats[CharacterColumn.Stat.PROJECTILE_AMOUNT] = initialStats[CharacterColumn.Stat.PROJECTILE_AMOUNT] = characterData.PROJECTILE_AMOUNT;
        
        price = characterData.PRICE;
    }

    // �Ű����� : ���� ����, ������
    public bool UpdateStats(CharacterColumn.Stat stat, float value)
    {
        // Maximum�� ���� ��츦 ����� bool�� ��ȯ
        stats[stat] += value;

        return true;
    }

    public bool UpdateStats(OptionColumn.Stat stat, OptionColumn.Type type, float value)
    {
        // Maximum�� ���� ��츦 ����� bool�� ��ȯ
        switch (type)
        {
            case OptionColumn.Type.Scale:
                stats[(CharacterColumn.Stat)stat] = stats[(CharacterColumn.Stat)stat] * (1.0f + value / 100.0f);
                break;
            case OptionColumn.Type.Fixed:
                stats[(CharacterColumn.Stat)stat] += value;
                break;
        }
        stats[CharacterColumn.Stat.DAMAGE_TYPE_1] += 10;

        return true;
    }
}
