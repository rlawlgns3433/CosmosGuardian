using CsvHelper;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyData
{
    public int MONSTER_ID { get; set; }
    public int TYPE { get; set; }
    public float HP { get; set; }
    public float DAMAGE { get; set; }
    public float MAGNIFICATION { get; set; }
    public int SCORE { get; set; }
    public int GOLD { get; set; }

    public EnemyData(EnemyData other)
    {
        this.MONSTER_ID = other.MONSTER_ID;
        this.TYPE = other.TYPE;
        this.HP = other.HP;
        this.DAMAGE = other.DAMAGE;
        this.MAGNIFICATION = other.MAGNIFICATION;
        this.SCORE = other.SCORE;
        this.GOLD = other.GOLD;
    }

    public EnemyData() { }

    public override string ToString()
    {
        return $"Monster id : {MONSTER_ID} / {TYPE} / {HP} / {DAMAGE} / {MAGNIFICATION} / {SCORE} / {GOLD}";
    }
}


public class EnemyTable : DataTable
{
    Dictionary<int, EnemyData> table = new Dictionary<int, EnemyData>();

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

            var records = csvReader.GetRecords<EnemyData>();
            foreach (var record in records)
            {
                table.Add(record.MONSTER_ID, record);
            }
        }
    }


    public EnemyData Get(int id)
    {
        if (!table.ContainsKey(id))
            return null;
        return table[id];
    }
}
