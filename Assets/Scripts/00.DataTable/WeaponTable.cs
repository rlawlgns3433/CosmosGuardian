using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public class WeaponData
{
    public int WEAPON_ID { get; set; }
    public string NAME_ID { get; set; }
    public string DESC_ID { get; set; }
    public int PREFAB_ID { get; set; }
    public int TYPE { get; set; }
    public int ANIM_TYPE { get; set; }
    public int PROJECTILE_ID { get; set; }
    public int PRICE { get; set; }
    public float DAMAGE { get; set; }
    public float FIRE_RATE { get; set; }
    public float FIRE_RANGE { get; set; }
    public int PENETRATE { get; set; }
    public float SPLASH_DAMAGE { get; set; }
    public float SPLASH_RANGE { get; set; }
    public float CRITICAL { get; set; }
    public float CRITICAL_DAMAGE { get; set; }
    public float HP_DRAIN { get; set; }
    public float PROJECTILE_SPEED { get; set; }
    public int PROJECTILE_AMOUNT { get; set; }

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
}

public class WeaponTable : DataTable
{
    Dictionary<int, WeaponData> table = new Dictionary<int, WeaponData>();

    public List<int> AllItemIds
    {
        get
        {
            return table.Keys.ToList();
        }
    }

    public List<WeaponData> AllItems
    {
        get
        {
            return table.Values.ToList();
        }
    }

    public Dictionary<int, WeaponData> AllData
    {
        get
        {
            return table;
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

            var records = csvReader.GetRecords<WeaponData>();
            foreach (var record in records)
            {
                table.Add(record.WEAPON_ID, record);
            }
        }
    }

    public WeaponData Get(int id)
    {
        if (!table.ContainsKey(id))
            return null;
        return table[id];
    }
}
