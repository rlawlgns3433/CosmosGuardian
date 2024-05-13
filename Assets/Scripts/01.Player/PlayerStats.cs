using System.Collections.Generic;
using System.Text;
using UnityEditor.UI;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int level = 1;
    private float exp;
    private float expForNextLevel = 100;

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
        /*
                 NONE,
        DAMAGE,
        FIRE_RATE,
        PENENTRATE,
        SPLASH_DAMAGE,
        SPLASH_RANGE,
        CRITICAL,
        CRITICAL_DAMAGE,
        HP_DRAIN,
        PROJECTILE_SPEED,
        PROJECTILE_AMOUNT,
        HP,
        MOVE_SPEED_V,
        MOVE_SPEED_H,
        ARMOR,
         */
        stats[CharacterColumn.Stat.HP] = initialStats[CharacterColumn.Stat.HP] = characterData.HP;
        stats[CharacterColumn.Stat.ARMOR] = initialStats[CharacterColumn.Stat.ARMOR] = characterData.ARMOR;
        stats[CharacterColumn.Stat.DAMAGE] = initialStats[CharacterColumn.Stat.DAMAGE] = characterData.DAMAGE_TYPE_1;
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
        stats[CharacterColumn.Stat.DAMAGE] += 10;

        return true;
    }

    public void GetExp(int score) // 스코어를 exp로 사용
    {
        exp += score;

        if(exp > expForNextLevel)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        // level up 효과 생성
        exp = 0;

    }
}
