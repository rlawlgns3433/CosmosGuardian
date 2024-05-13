public static class DataTableIds
{
    public static readonly string[] String =
    {
        "STRING_TABLE",
        "StringTableEn",
        "StringTableJp"
    };

    public static readonly string Character = "CHARACTER_TABLE";
    public static readonly string Enemy = "MONSTER_TABLE";
    public static readonly string Option = "OptionTable";
    public static readonly string Weapon = "WEAPON_TABLE";
    public static string CurrString
    {
        get
        {
            return String[(int)Vars.currentLang];
        }
    }
}

public static class Vars
{
    public static readonly string Version = "1.0.0";
    public static readonly int BuildVersion = 8;

    public static Languages currentLang = Languages.Korean;

    public static Languages editorLang = Languages.Korean;
}

public static class Tags
{
    public static readonly string Enemy = "Enemy";
    public static readonly string Player = "Player";
    public static readonly string GameController = "GameController";
}

public static class SortingLayers
{
    public static readonly string Default = "Default";
}

public static class Layers
{
    public static readonly string UI = "UI";
}

public enum Languages
{
    Korean,
    English,
    Japanese,
}

public static class OptionColumn
{
    public enum Type
    { 
        Scale,
        Fixed
    }
     
    public enum Stat
    {
        DAMAGE,
        FIRE_RATE,
        FIRE_RANGE,
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
    }
}

public static class CharacterColumn
{
    public enum CharacterPrefabNumber
    {
        Jamse,
        Anna
    }
    public enum Stat
    {
        DAMAGE,
        FIRE_RATE,
        FIRE_RANGE,
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
    }
}

public enum TableIdentifier
{
    Option = 1, // id + type + stat(D2) + grade(D2)
    Character = 2, // id + body(D2) + head(D2)
}