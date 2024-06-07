
using System.Collections.Generic;

public class CharacterStat
{
    public Dictionary<CharacterColumn.Stat, float> stat;
    public Dictionary<CharacterColumn.Stat, float> uiStat;
    public CharacterData characterData;
    public UiCharacterData uiCharacterData;

    public CharacterStat()
    {
        stat = new Dictionary<CharacterColumn.Stat, float>();
        uiStat = new Dictionary<CharacterColumn.Stat, float>();
    }

    public CharacterStat(int characterDataId) : this()
    {
        characterData = DataTableMgr.Get<CharacterTable>(DataTableIds.Character).Get(characterDataId);
        uiCharacterData = DataTableMgr.Get<CharacterTable>(DataTableIds.Character).GetUiData(characterDataId);
        SetOriginalStat();
        SetUiStat();
    }

    public void Assign(CharacterStat other)
    {
        if (characterData == null && uiCharacterData == null)
        {
            stat = new Dictionary<CharacterColumn.Stat, float>();
            uiStat = new Dictionary<CharacterColumn.Stat, float>();
        }

        characterData = other.characterData;
        uiCharacterData = other.uiCharacterData;
        SetOriginalStat();
        SetUiStat();
    }

    public void Assign(CharacterData other)
    {
        Assign(other.CHARACTER_ID);
    }

    public void Assign(int otherId)
    {
        if (characterData == null && uiCharacterData == null)
        {
            stat = new Dictionary<CharacterColumn.Stat, float>();
            uiStat = new Dictionary<CharacterColumn.Stat, float>();
        }

        characterData = DataTableMgr.Get<CharacterTable>(DataTableIds.Character).Get(otherId);
        uiCharacterData = DataTableMgr.Get<CharacterTable>(DataTableIds.Character).GetUiData(otherId);

        SetOriginalStat();
        SetUiStat();
    }

    public void SetOriginalStat()
    {
        stat[CharacterColumn.Stat.HP] = characterData.HP;
        stat[CharacterColumn.Stat.ARMOR] = characterData.ARMOR * 100 - 100;
        stat[CharacterColumn.Stat.DAMAGE] = characterData.DAMAGE_TYPE_1 * 100;
        stat[CharacterColumn.Stat.MOVE_SPEED_V] = characterData.MOVE_SPEED_V * 100;
        stat[CharacterColumn.Stat.MOVE_SPEED_H] = characterData.MOVE_SPEED_H * 100;
        stat[CharacterColumn.Stat.FIRE_RATE] = characterData.FIRE_RATE * 100;
        stat[CharacterColumn.Stat.FIRE_RANGE] = characterData.FIRE_RANGE * 100;
        stat[CharacterColumn.Stat.PENETRATE] = characterData.PENETRATE * 100;
        stat[CharacterColumn.Stat.SPLASH_DAMAGE] = characterData.SPLASH_DAMAGE * 100;
        stat[CharacterColumn.Stat.SPLASH_RANGE] = characterData.SPLASH_RANGE * 100;
        stat[CharacterColumn.Stat.CRITICAL] = characterData.CRITICAL * 100;
        stat[CharacterColumn.Stat.CRITICAL_DAMAGE] = characterData.CRITICAL_DAMAGE * 100;
        stat[CharacterColumn.Stat.HP_DRAIN] = characterData.HP_DRAIN * 100;
        stat[CharacterColumn.Stat.PROJECTILE_SPEED] = characterData.PROJECTILE_SPEED * 100;
        stat[CharacterColumn.Stat.PROJECTILE_AMOUNT] = characterData.PROJECTILE_AMOUNT * 100;
    }

    public void SetUiStat()
    {
        uiStat[CharacterColumn.Stat.HP] = uiCharacterData.HP;
        uiStat[CharacterColumn.Stat.ARMOR] = uiCharacterData.ARMOR;
        uiStat[CharacterColumn.Stat.DAMAGE] = uiCharacterData.DAMAGE_TYPE_1;
        uiStat[CharacterColumn.Stat.MOVE_SPEED_V] = uiCharacterData.MOVE_SPEED_V;
        uiStat[CharacterColumn.Stat.MOVE_SPEED_H] = uiCharacterData.MOVE_SPEED_H;
        uiStat[CharacterColumn.Stat.FIRE_RATE] = uiCharacterData.FIRE_RATE;
        uiStat[CharacterColumn.Stat.FIRE_RANGE] = uiCharacterData.FIRE_RANGE;
        uiStat[CharacterColumn.Stat.PENETRATE] = uiCharacterData.PENETRATE;
        uiStat[CharacterColumn.Stat.SPLASH_DAMAGE] = uiCharacterData.SPLASH_DAMAGE;
        uiStat[CharacterColumn.Stat.SPLASH_RANGE] = uiCharacterData.SPLASH_RANGE;
        uiStat[CharacterColumn.Stat.CRITICAL] = uiCharacterData.CRITICAL;
        uiStat[CharacterColumn.Stat.CRITICAL_DAMAGE] = uiCharacterData.CRITICAL_DAMAGE;
        uiStat[CharacterColumn.Stat.HP_DRAIN] = uiCharacterData.HP_DRAIN;
        uiStat[CharacterColumn.Stat.PROJECTILE_SPEED] = uiCharacterData.PROJECTILE_SPEED;
        uiStat[CharacterColumn.Stat.PROJECTILE_AMOUNT] = uiCharacterData.PROJECTILE_AMOUNT;
    }
}