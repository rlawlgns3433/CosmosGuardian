using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class DataTableMgr
{
    private static Dictionary<string, DataTable> tables = new Dictionary<string, DataTable>();

    static DataTableMgr()
    {
        //foreach (var id in DataTableIds.String)
        //{
        //    DataTable table = new StringTable();
        //    table.Load(id);
        //    tables.Add(id, table);
        //}

        OptionTable optionTable = new OptionTable();
        optionTable.Load(DataTableIds.Option);
        tables.Add(DataTableIds.Option, optionTable);

        CharacterTable characterTable = new CharacterTable();
        characterTable.Load(DataTableIds.Character);
        tables.Add(DataTableIds.Character, characterTable);

        EnemyTable enemyTable = new EnemyTable();
        enemyTable.Load(DataTableIds.Enemy);
        tables.Add(DataTableIds.Enemy, enemyTable);
    }

    public static StringTable GetStringTable()
    {
        return Get<StringTable>(DataTableIds.String[(int)Vars.currentLang]);
    }

    public static T Get<T>(string id) where T : DataTable
    {
        if (!tables.ContainsKey(id))
            return null;
        return tables[id] as T;
    }
}
