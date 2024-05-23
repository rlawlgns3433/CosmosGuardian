using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public List<GameObject> platforms = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>(); // GameManager에서 갖는 것이 좋겠음.
    public EnemyTable enemyTable;
    public PlayerStats playerStats = null;
    public UiController uiController;

    public int currentPlatformIndex = 0;
    public int nextPlatformIndex = 1;

    public bool IsGameover { get; set; }
    public bool IsPaused { get; set; }

    public float platformSpacing = 27f;

    private void Awake()
    {
        enemyTable = DataTableMgr.Get<EnemyTable>(DataTableIds.Enemy);
    }

    private void Update()
    {
        GameObject currentPlatform = platforms[currentPlatformIndex];
        if (currentPlatform.transform.position.z + platformSpacing < playerStats.gameObject.transform.position.z)
        {
            Vector3 lastPlatformPosition = platforms[platforms.Count - 1].transform.position;
            Vector3 newPlatformPosition = lastPlatformPosition + new Vector3(0, 0, platformSpacing);
            currentPlatform.transform.position = newPlatformPosition;

            platforms.Remove(currentPlatform);
            platforms.Add(currentPlatform);

            var platform = currentPlatform.GetComponent<Platform>();
            platform.ResetPlatform();
        }
    }

    public void Gameover()
    {
        IsGameover = true;
        playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_H] = 0;
        playerStats.stats[CharacterColumn.Stat.MOVE_SPEED_V] = 0;
        playerStats.gameObject.GetComponent<PlayerShooter>().enabled = false;
        playerStats.gameObject.GetComponent<PlayerInput>().enabled = false;

        Collider[] colliders = playerStats.gameObject.GetComponents<Collider>();

        foreach (var collider in colliders)
        {
            collider.enabled = false;
        }

        GameObject.FindWithTag(Tags.Joystick).SetActive(false);
        uiController.gameover.SetActive(true);
    }

    public void Pasue()
    {
        if (!IsPaused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
        IsPaused = !IsPaused;
    }

    public void EnterMainScene()
    {
        Time.timeScale = 1f;
        LoadScene("Main");
    }

    public void LoadScene(string sceneName)
    {
        if (IsGameover)
        {
            RecordData record = new RecordData();
            record.characterDataId = playerStats.characterData.CHARACTER_ID;
            record.weaponDataId = playerStats.playerShooter.weapon.weaponData.WEAPON_ID;
            record.score = playerStats.exp;

            SaveRecord(record);
        }

        ParamManager.SceneToLoad = sceneName; // 로드할 씬 이름 저장
        SceneManager.LoadScene("Loading"); // 로딩 씬 로드
    }

    public void SaveRecord(RecordData recordData)
    {
        ParamManager.currentRecord = recordData;
    }
}
