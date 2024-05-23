using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public static class SaveLoadSystem 
{
    public enum Mode
    {
        Json,
        Binary,
        EncryptedBinary,
    }

    public static Mode FileMode { get; set; } = Mode.Json;

    public static int SaveDataVersion { get; private set; } = 1;

    // 0 (자동), 1, 2, 3 ...
    private static readonly string[] SaveFileName =
    {
        "SaveAuto.sav",
        "Save1.sav",
        "Save2.sav",
        "Save3.sav"
    };

    private static string SaveDirectory
    {
        get
        {
            return $"{Application.persistentDataPath}/Save";
        }
    }


    public static bool Save(SaveData data, int slot = 0)
    {
        if (slot < 0 ||  slot >= SaveFileName.Length)
        {
            return false;
        }

        if (!Directory.Exists(SaveDirectory))
        {
            Directory.CreateDirectory(SaveDirectory);
        }

        var path = Path.Combine(SaveDirectory, SaveFileName[slot]);
        // FileMode 분기

        using (var writer = new JsonTextWriter(new StreamWriter(path)))
        {
            var serializer = new JsonSerializer();
            serializer.Formatting = Formatting.Indented;
            serializer.TypeNameHandling = TypeNameHandling.All;
            serializer.Converters.Add(new RecordConverter());
            serializer.Serialize(writer, data);
        }

        return true;
    }

    public static SaveData Load(int slot = 0)
    {
        if (slot < 0 ||  slot >= SaveFileName.Length)
        {
            return null;
        }
        var path = Path.Combine(SaveDirectory, SaveFileName[slot]);
        if (!File.Exists(path))
        {
            return null;
        }

        SaveData data = null;
        using (var reader = new JsonTextReader(new StreamReader(path)))
        {
            var serializer = new JsonSerializer();
            //serializer.Formatting = Formatting.Indented;
            serializer.TypeNameHandling = TypeNameHandling.All;
            serializer.Converters.Add(new RecordConverter());
            data = serializer.Deserialize<SaveData>(reader);
        }

        while (data.Version < SaveDataVersion)
        {
            data = data.VersionUp();
        }

        return data;
    }

}
