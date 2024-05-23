using System.Collections.Generic;

public abstract class SaveData
{
    public int Version { get; protected set; }

    public abstract SaveData VersionUp();

}

public class RecordData
{
    public int characterDataId;
    public int weaponDataId;
    public int score;

    public RecordData() { }
    public RecordData(int characterDataId, int weaponDataId, int score)
    {
        this.characterDataId = characterDataId;
        this.weaponDataId = weaponDataId;
        this.score = score;
    }
}

public class SaveDataV1 : SaveData
{
    public List<RecordData> records = new List<RecordData>();

    public SaveDataV1()
    {
        Version = 1;
    }
    public override SaveData VersionUp()
    {
        return null;
    }
}

//public class SaveDataV1 : SaveData
//{
//    public int Gold { get; set; } = 100;

//    public SaveDataV1()
//    {
//        Version = 1;
//    }

//    public override SaveData VersionUp()
//    {
//        var data = new SaveDataV2();
//        data.Gold = Gold;
//        return data;
//    }
//}

public class SaveDataV2 : SaveData
{
    public int Gold { get; set; } = 100;
    public string Name { get; set; } = "Empty";

    public  SaveDataV2()
    {
        Version = 2;
    }

    public override SaveData VersionUp()
    {
        return null;
    }
}

/*
public class SaveDataV3 : SaveData
{
    public int Gold { get; set; } = 100;
    //public string Name { get; set; } = "Empty";

    public Vector3 Position { get; set; } = Vector3.zero;
    public Quaternion Rotation { get; set; }= Quaternion.identity;
    public Vector3 Scale { get; set; } = Vector3.one;

    public Color color { get; set; } = Color.white;
}
*/
