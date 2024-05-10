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
    public string DESC_ID { get; set; }
    public CharacterColumn.Stat STAT_1 { get; set; }
    public float VALUE_1 { get; set; }
    public CharacterColumn.Stat STAT_2 { get; set; }
    public float VALUE_2 { get; set; }
    public CharacterColumn.Stat STAT_3 { get; set; }
    public float VALUE_3 { get; set; }
    public CharacterColumn.Stat STAT_4 { get; set; }
    public float VALUE_4 { get; set; }


    public string GetName
    {
        get
        {
            return DataTableMgr.GetStringTable().Get(NAME_ID);
        }
    }

    public string GetDESC
    {
        get
        {
            return DataTableMgr.GetStringTable().Get(DESC_ID);
        }
    }

    public override string ToString()
    {
        return $"CHARACTER_ID : {CHARACTER_ID}\nNAME : {GetName}\nHP : {HP}"; // String 테이블이 없으므로 현재 GetName 에러 발생 
    }
}

public class CharacterTable : DataTable
{
    Dictionary<int, CharacterData> table = new Dictionary<int, CharacterData>();

    public List<int> AllItemIds
    {
        get
        {
            return table.Keys.ToList();
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
            }
        }
    }

    public CharacterData Get(int id)
    {
        if (!table.ContainsKey(id))
            return null;
        return table[id];
    }
}
