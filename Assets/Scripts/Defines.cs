using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;

public static class DataTableIds
{
    public static readonly string[] String =
    {
        "StringTableKr",
        "StringTableEn",
        "StringTableJp"
    };

    public static readonly string Item = "ItemTable";
    public static readonly string Character = "CharacterTable";
    public static readonly string Option = "OptionTable";
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
        Damage,
        FireRate,
        FireRange,
        Penentrate,
        SplashDamage,
        SplashRange,
        Critical,
        CriticalDamage,
        HpDrain,
        ProjectileSpeed,
        ProjectileAmount,
        Hp,
        MoveSpeedVertical,
        MoveSpeedHorizontal,
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
        None,
        FireRate,
        FireRange,
        SplashDamage,
        SplashRange,
        Critical,
        CriticalDamage,
        HpDrain,
        ProjectileSpeed,
    }
}


public enum TableIdentifier
{
    Option = 1, // id + type + stat(D2) + grade(D2)
    Character = 2, // id + body(D2) + head(D2)
}