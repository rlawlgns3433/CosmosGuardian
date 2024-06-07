using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using CsvHelper;
using UnityEngine;
public class CharacterData
{
    public int CHARACTER_ID { get; set; }
    public string NAME_ID { get; set; }
    public string DESC_ID { get; set; }
    public int BODY_TYPE { get; set; }
    public int HEAD_TYPE { get; set; }
    public int PRICE { get; set; }
    public float HP { get; set; }
    public float ARMOR { get; set; }
    public float DAMAGE_TYPE_1 { get; set; }
    public float DAMAGE_TYPE_2 { get; set; }
    public float DAMAGE_TYPE_3 { get; set; }
    public float MOVE_SPEED_V { get; set; }
    public float MOVE_SPEED_H { get; set; }
    public float FIRE_RATE { get; set; }
    public float FIRE_RANGE { get; set; }
    public float PENETRATE { get; set; }
    public float SPLASH_DAMAGE { get; set; }
    public float SPLASH_RANGE { get; set; }
    public float CRITICAL { get; set; }
    public float CRITICAL_DAMAGE { get; set; }
    public float HP_DRAIN { get; set; }
    public float PROJECTILE_SPEED { get; set; }
    public float PROJECTILE_AMOUNT { get; set; }

    public string GetName
    {
        get
        {
            return DataTableMgr.GetStringTable().Get(NAME_ID);
        }
    }

    public string GetDesc
    {
        get
        {
            return DataTableMgr.GetStringTable().Get(DESC_ID);
        }
    }

    public override string ToString()
    {
        return $"CHARACTER_ID : {CHARACTER_ID}\nNAME : {GetName}\nHP : {HP}"; // String ���̺��� �����Ƿ� ���� GetName ���� �߻� 
    }
}

public class UiCharacterData
{
    public UiCharacterData(
        int characterId,
        float hp,
        float armor,
        float damageType1,
        float moveSpeedV,
        float moveSpeedH,
        float fireRate,
        float fireRange,
        float penetrate,
        float splashDamage,
        float splashRange,
        float critical,
        float criticalDamage,
        float hpDrain,
        float projectileSpeed,
        float projectileAmount)
    {
        CHARACTER_ID = characterId;
        HP = hp;
        ARMOR = armor;
        DAMAGE_TYPE_1 = damageType1;
        MOVE_SPEED_V = moveSpeedV;
        MOVE_SPEED_H = moveSpeedH;
        FIRE_RATE = fireRate;
        FIRE_RANGE = fireRange;
        PENETRATE = penetrate;
        SPLASH_DAMAGE = splashDamage;
        SPLASH_RANGE = splashRange;
        CRITICAL = critical;
        CRITICAL_DAMAGE = criticalDamage;
        HP_DRAIN = hpDrain;
        PROJECTILE_SPEED = projectileSpeed;
        PROJECTILE_AMOUNT = projectileAmount;
    }

    public int CHARACTER_ID { get; set; }
    public float HP { get; set; }
    public float ARMOR { get; set; }
    public float DAMAGE_TYPE_1 { get; set; }
    public float MOVE_SPEED_V { get; set; }
    public float MOVE_SPEED_H { get; set; }
    public float FIRE_RATE { get; set; }
    public float FIRE_RANGE { get; set; }
    public float PENETRATE { get; set; }
    public float SPLASH_DAMAGE { get; set; }
    public float SPLASH_RANGE { get; set; }
    public float CRITICAL { get; set; }
    public float CRITICAL_DAMAGE { get; set; }
    public float HP_DRAIN { get; set; }
    public float PROJECTILE_SPEED { get; set; }
    public float PROJECTILE_AMOUNT { get; set; }
}


public class CharacterTable : DataTable
{
    Dictionary<int, CharacterData> table = new Dictionary<int, CharacterData>();
    Dictionary<int, UiCharacterData> uiTable = new Dictionary<int, UiCharacterData>(); 

    public List<int> AllItemIds
    {
        get
        {
            return table.Keys.ToList();
        }
    }

    public Dictionary<int, CharacterData> AllData
    {
        get
        {
            return table;
        }
    }

    public UiCharacterData StandardCharacterData
    {
        get
        {
            return GetUiData(20101);
        }
    }


    public override void Load(string path)
    {
        path = string.Format(FormatPath, path);

        table.Clear();

        var textAsset = Resources.Load<TextAsset>(path);

        using (var reader = new StringReader(textAsset.text))
        using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
        {
            csvReader.Read();
            csvReader.ReadHeader();
            csvReader.Read();

            var records = csvReader.GetRecords<CharacterData>();
            foreach (var record in records)
            {
                table.Add(record.CHARACTER_ID, record);
                uiTable.Add(record.CHARACTER_ID, 
                    new UiCharacterData(
                    record.CHARACTER_ID, 
                    record.HP,
                    record.ARMOR * 100 - 100, 
                    record.DAMAGE_TYPE_1 * 100, 
                    record.MOVE_SPEED_V * 100,
                    record.MOVE_SPEED_H * 100, 
                    record.FIRE_RATE * 100, 
                    record.FIRE_RANGE * 100, 
                    record.PENETRATE * 100, 
                    record.SPLASH_DAMAGE * 100, 
                    record.SPLASH_RANGE * 100,
                    record.CRITICAL * 100, 
                    record.CRITICAL_DAMAGE * 100, 
                    record.HP_DRAIN * 100, 
                    record.PROJECTILE_SPEED * 100, 
                    record.PROJECTILE_AMOUNT * 100));
            }
        }
    }

    public CharacterData Get(int id)
    {
        if (!table.ContainsKey(id))
            return null;
        return table[id];
    }

    public UiCharacterData GetUiData(int id)
    {
        if (!uiTable.ContainsKey(id))
            return null;

        return uiTable[id];
    }
}
