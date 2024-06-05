using CsvHelper;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public class OptionData
{
    public int UPGRADE_ID { get; set; }
    public string STRING_ID { get; set; }
    //public string NAME_DEV { get; set; }
    public OptionColumn.Type TYPE { get; set; }
    public OptionColumn.Stat STAT { get; set; }
    public int GRADE { get; set; }
    public float VALUE { get; set; }

    public string GetName
    {
        get
        {
            return DataTableMgr.GetStringTable().Get(STRING_ID);
        }
    }
    public override string ToString()
    {
        return $"UPGRADE_ID : {UPGRADE_ID}\nNAME_DEV : {GetName}\nTYPE : {TYPE}\nSTAT : {STAT}\nGRADE : {GRADE}\nVALUE : {VALUE}";
    }
}

public class OptionTable : DataTable
{
    Dictionary<int, OptionData> table = new Dictionary<int, OptionData>();

    public int KeyCount
    {
        get
        {
            return table.Keys.Count;
        }
    }

    public List<int> GetKeys
    {
        get
        {
            return table.Keys.ToList();
        }
    }

    public List<OptionData> GetAllData
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
            csvReader.Read(); // 첫 번째 행 읽기 (헤더)
            csvReader.ReadHeader(); // 헤더 읽기
            csvReader.Read(); // 두 번째 행 건너뛰기

            var records = csvReader.GetRecords<OptionData>();
            foreach (var record in records)
            {
                table.Add(record.UPGRADE_ID, record);
            }
        }
    }

    public OptionData Get(int UPGRADE_ID)
    {
        if (!table.ContainsKey(UPGRADE_ID))
            return null;
        return table[UPGRADE_ID];
    }
}