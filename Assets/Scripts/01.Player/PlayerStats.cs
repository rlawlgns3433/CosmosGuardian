using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Text;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    private CharacterTable characterTable = null;
    private Dictionary<CharacterColumn.Stat, float> initialStats = new Dictionary<CharacterColumn.Stat, float>(); // 능력치 종류, 능력치 배율
    public Dictionary<CharacterColumn.Stat, float> stats = new Dictionary<CharacterColumn.Stat, float>(); // 능력치 종류, 능력치 배율
    private PrefabSelector prefabSelector = null;
    public CharacterData characterData = null;

    // 이 부분을 Scriptable Assets로 변경할 수도 있음
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

        stats[CharacterColumn.Stat.Hp] = initialStats[CharacterColumn.Stat.Hp] = characterData.HP;
        stats[CharacterColumn.Stat.Armor] = initialStats[CharacterColumn.Stat.Armor] = characterData.ARMOR;
        stats[CharacterColumn.Stat.DamageType1] = initialStats[CharacterColumn.Stat.DamageType1] = characterData.DAMAGE_TYPE_1;
        stats[CharacterColumn.Stat.DamageType2] = initialStats[CharacterColumn.Stat.DamageType2] = characterData.DAMAGE_TYPE_2;
        stats[CharacterColumn.Stat.DamageType3] = initialStats[CharacterColumn.Stat.DamageType3] = characterData.DAMAGE_TYPE_3;
        stats[CharacterColumn.Stat.SpeedVertical] = initialStats[CharacterColumn.Stat.SpeedVertical] = characterData.MOVE_SPEED_V;
        stats[CharacterColumn.Stat.SpeedHorizontal] = initialStats[CharacterColumn.Stat.SpeedHorizontal] = characterData.MOVE_SPEED_H;
        
        price = characterData.PRICE;
    }

    // 매개변수 : 스텟 종류, 증가량
    public bool UpdateStats(CharacterColumn.Stat stat, float value)
    {
        // Maximum이 있을 경우를 고려해 bool을 반환
        stats[stat] += value;

        return true;
    }

    public bool UpdateStats(OptionColumn.Stat stat, OptionColumn.Type type, float value)
    {
        // Maximum이 있을 경우를 고려해 bool을 반환
        switch (type)
        {
            case OptionColumn.Type.Scale:
                stats[(CharacterColumn.Stat)stat] = stats[(CharacterColumn.Stat)stat] * (1.0f + value / 100.0f);
                break;
            case OptionColumn.Type.Fixed:
                stats[(CharacterColumn.Stat)stat] += value;
                break;
        }

        return true;
    }
}
