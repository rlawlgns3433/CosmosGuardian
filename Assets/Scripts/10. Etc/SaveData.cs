using System.Collections.Generic;

public abstract class SaveData
{
    public int Version { get; protected set; }

    public abstract SaveData VersionUp();
}

public class PlayerOption
{
    public float bgmValue = -1;
    public float sfxValue = -1;
    public bool isCameraShake = false;

    public PlayerOption() 
    {
        bgmValue = -1;
        sfxValue = -1;
        isCameraShake = false;
    }
    public PlayerOption(float bgmValue, float sfxValue, bool isCameraShake)
    {
        this.bgmValue = bgmValue;
        this.sfxValue = sfxValue;
        this.isCameraShake = isCameraShake;
    }
}

public class RecordData
{
    public int characterDataId;
    public int weaponDataId;
    public int score;

    public RecordData()
    {
        characterDataId = 0;
        weaponDataId = 0;
        score = -1;
    }
    public RecordData(int characterDataId, int weaponDataId, int score)
    {
        this.characterDataId = characterDataId;
        this.weaponDataId = weaponDataId;
        this.score = score;
    }
}

public class SaveDataV1 : SaveData
{
    public List<RecordData> records;
    public PlayerOption playerOption;

    public SaveDataV1()
    {
        records = new List<RecordData>();
        playerOption = new PlayerOption();
        Version = 1;
    }
    public override SaveData VersionUp()
    {
        return null;
    }
}

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