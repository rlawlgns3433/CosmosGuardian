using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public class ItemData
{
    public static readonly string FormatIconPath = "Skills/{0}";

    public int ITEM_ID { get; set; }
    public int TYPE { get; set; }
    public int GRADE { get; set; }
    public int OPTION_1 { get; set; }
    public int OPTION_2 { get; set; }
    public int OPTION_3 { get; set; }
    public string STRING_ID { get; set; }
    public string ICON_ID { get; set; }

    public OptionData GetOption1
    {
        get
        {
            return DataTableMgr.GetOptionTable().Get(OPTION_1);
        }
    }
    
    public OptionData GetOption2
    {
        get
        {
            return DataTableMgr.GetOptionTable().Get(OPTION_2);
        }
    }
    
    public OptionData GetOption3
    {
        get
        {
            if (OPTION_3 == 0)
                return default(OptionData);

            return DataTableMgr.GetOptionTable().Get(OPTION_3);
        }
    }

    public string GetString
    {
        get
        {
            return DataTableMgr.GetStringTable().Get(STRING_ID);
        }
    }

    public Sprite GetSprite
    {
        get
        {
            return Resources.Load<Sprite>(string.Format(FormatIconPath, ICON_ID));
        }
    }

    public int GetIconIdInt
    {
        get
        {
            return ITEM_ID % 1000;
        }
    }
}

public class ItemTable : DataTable
{
    Dictionary<int, ItemData> table = new Dictionary<int, ItemData>();

    public List<int> AllItemIds
    {
        get
        {
            return table.Keys.ToList();
        }
    }

    public List<int> GetKeys
    {
        get
        {
            return table.Keys.ToList();
        }
    }

    public List<ItemData> GetAllData
    {
        get
        {
            return table.Values.ToList();
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

            var records = csvReader.GetRecords<ItemData>();
            foreach (var record in records)
            {
                table.Add(record.ITEM_ID, record);
            }
        }
    }

    public ItemData Get(int id)
    {
        if (!table.ContainsKey(id))
            return null;
        return table[id];
    }
}
