using UnityEngine;
using UnityEngine.SceneManagement;

public static class ParamManager
{
    public static SaveDataV1 saveData;
    public static RecordData currentRecord = new RecordData();
    public static SceneIds SceneToLoad = SceneIds.None;
    public static int selectedWeaponIndex = 11; // 시작 무기
    private static int selectedCharacterIndex = -1;
    public static int SelectedCharacterIndex
    { 
        get
        {
            if(selectedCharacterIndex == -1)
            {
                selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0) % 100;
            }

            return selectedCharacterIndex;
        }
        set
        {
            selectedCharacterIndex = value;
        }

    }

    private static bool isCameraShaking = true;

    public static bool IsCameraShaking
    {
        get 
        {
            GenerateSaveData();
            return saveData.playerOption.isCameraShake; 
        }
        set
        {
            saveData.playerOption.isCameraShake = value;
            isCameraShaking = value;
        }
    }
    private static float bgmValue = 1f;
    public static float BgmValue
    {
        get 
        {
            GenerateSaveData();
            return saveData.playerOption.bgmValue; 
        }
        set
        {
            saveData.playerOption.bgmValue = value;
            bgmValue = value;
        }
    }
    private static float sfxValue = 1f;
    public static float SfxValue
    {
        get 
        {
            GenerateSaveData();
            return saveData.playerOption.sfxValue; 
        }
        set
        {
            saveData.playerOption.sfxValue = value;
            sfxValue = value;
        }
    }

    public static System.Comparison<RecordData> comparison =
        (RecordData x, RecordData y) =>
        {
            return y.score.CompareTo(x.score);
        };


    public static void SaveCurrentRecord(RecordData recordData)
    {
        GenerateSaveData();

        currentRecord = recordData;
        saveData.records.Add(currentRecord);
        saveData.records.Sort(comparison);

        saveData.playerOption.bgmValue = bgmValue;
        saveData.playerOption.sfxValue = sfxValue;
        saveData.playerOption.isCameraShake = isCameraShaking;

        if (saveData.records.Count > 5)
        {
            saveData.records.RemoveAt(saveData.records.Count - 1);
        }

        SaveLoadSystem.Save(saveData);
        currentRecord.weaponDataId = 0;
        currentRecord.characterDataId = 0;
        currentRecord.score = -1;
    }

    public static void LoadScene(SceneIds sceneName)
    {
        SceneToLoad = sceneName;
        SceneManager.LoadScene((int)SceneIds.Loading);
    }

    public static void GenerateSaveData()
    {
        if (saveData == null)
        {
            saveData = SaveLoadSystem.Load() as SaveDataV1;
            if (saveData == null)
            {
                saveData = new SaveDataV1();
                saveData.playerOption.bgmValue = bgmValue;
                saveData.playerOption.sfxValue = sfxValue;
                saveData.playerOption.isCameraShake = IsCameraShaking;
                SaveLoadSystem.Save(saveData);
            }
        }
    }
}