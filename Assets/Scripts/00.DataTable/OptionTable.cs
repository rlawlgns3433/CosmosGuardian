using CsvHelper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using UnityEngine;

public class OptionData
{
    public int UPGRADE_ID { get; set; }
    public string NAME_DEV { get; set; }
    public int TYPE { get; set; }
    public int STAT { get; set; }
    public int GRADE { get; set; }
    public int VALUE { get; set; }
    public override string ToString()
    {
        return $"UPGRADE_ID : {UPGRADE_ID}\nNAME_DEV : {NAME_DEV}\nTYPE : {TYPE}\nSTAT : {STAT}\nGRADE : {GRADE}\nVALUE : {VALUE}";
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
                Debug.Log(record);
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