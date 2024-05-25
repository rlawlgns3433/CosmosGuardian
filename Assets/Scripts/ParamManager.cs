using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class ParamManager : MonoBehaviour
{
    public static SaveDataV1 saveData = new SaveDataV1();
    public static RecordData currentRecord = new RecordData();
    public static int selectedCharacterIndex = 0;
    public static int selectedWeaponIndex = 0;
    public static bool IsCameraShaking = true;
    public static float BgmValue = 1;
    public static float SfxValue = 1;
    public static int playerScore = 0;
    public static string SceneToLoad = string.Empty;

    public static System.Comparison<RecordData> comparison =
        (RecordData x, RecordData y) =>
        {
            return y.score.CompareTo(x.score);
        };

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public static void SaveCurrentRecord(RecordData recordData)
    {
        saveData = SaveLoadSystem.Load() as SaveDataV1;
        if (saveData == null)
        {
            saveData = new SaveDataV1();
        }
        currentRecord = recordData;
        saveData.records.Add(currentRecord);
        saveData.records.Sort(comparison);

        if(saveData.records.Count > 5)
        {
            saveData.records.RemoveAt(saveData.records.Count - 1);
        }

        SaveLoadSystem.Save(saveData);
        currentRecord = default;
    }

    public static void LoadScene(string sceneName)
    {
        SceneToLoad = sceneName;
        SceneManager.LoadScene("Loading"); // ·Îµù ¾À ·Îµå
    }
}
