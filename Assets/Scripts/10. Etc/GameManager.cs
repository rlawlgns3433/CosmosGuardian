using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private int victory = Animator.StringToHash("Victory");

    public List<GameObject> platforms = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>();
    public EnemyTable enemyTable;
    public PlayerStats playerStats = null;
    public PlayerHealth playerHealth;
    public UiController uiController;

    public int currentPlatformIndex = 0;
    public int nextPlatformIndex = 1;

    public bool IsGameover { get; set; }
    public bool IsPaused { get; set; }
    public bool isFirst = true;
    public bool isChasing = false;

    public float platformSpacing = 27f;

    private void Awake()
    {
        enemyTable = DataTableMgr.Get<EnemyTable>(DataTableIds.Enemy);
    }

    private void Update()
    {

        GameObject currentPlatform = platforms[currentPlatformIndex];

        if (isFirst)
        {
            var platform = currentPlatform.GetComponent<Platform>();

            if (currentPlatform.transform.position.z < playerStats.gameObject.transform.position.z)
            {

                foreach (var enemies in platform.spawnedEnemies.Values)
                {
                    foreach (var enemy in enemies)
                    {
                        enemy.Chase();
                    }
                }
                isFirst = false;
            }
            return;
        }

        if (currentPlatform.transform.position.z + platformSpacing < playerHealth.gameObject.transform.position.z)
        {
            Platform nextPlatform = platforms[nextPlatformIndex].GetComponent<Platform>();
            foreach (var enemies in nextPlatform.spawnedEnemies.Values)
            {
                foreach (var enemy in enemies)
                {
                    if(!enemy.isChasing)
                        enemy.Chase();
                }
            }
        }


        if (currentPlatform.transform.position.z + platformSpacing < Camera.main.gameObject.transform.position.z)
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

        foreach (var platformGo in platforms)
        {
            var platform = platformGo.GetComponent<Platform>();
            foreach (var enemies in platform.spawnedEnemies.Values)
            {
                foreach (var enemy in enemies)
                {
                    if (enemy != null && enemy.gameObject.activeInHierarchy)
                    {
                        enemy.animator.SetTrigger(victory);
                    }
                }
            }
        }

        playerStats.stats.stat[CharacterColumn.Stat.MOVE_SPEED_H] = 0;
        playerStats.stats.stat[CharacterColumn.Stat.MOVE_SPEED_V] = 0;
        playerStats.gameObject.GetComponent<PlayerShooter>().enabled = false;

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

        foreach(var text in DynamicTextManager.usingText)
        {
            Destroy(text);
        }
        foreach (var text in DynamicTextManager.unusingText)
        {
            Destroy(text);
        }
        DynamicTextManager.usingText.Clear();
        DynamicTextManager.unusingText.Clear();


        LoadScene(SceneIds.Main);
    }

    public void LoadScene(SceneIds sceneName)
    {
        if (IsGameover)
        {
            RecordData record = new RecordData();
            record.characterDataId = playerStats.characterData.CHARACTER_ID;
            record.weaponDataId = playerStats.playerShooter.weapon.weaponData.WEAPON_ID;
            record.score = playerStats.exp;
            GPGSMgr.ReportScore(record.score);
            SaveRecord(record);
        }

        ParamManager.SceneToLoad = sceneName;
        SceneManager.LoadScene((int)SceneIds.Loading);
    }

    public void SaveRecord(RecordData recordData)
    {
        ParamManager.currentRecord = recordData;
    }

    private void OnApplicationQuit()
    {
#if UNITY_EDITOR
        PlayerPrefs.SetInt("SelectedCharacterIndex", ParamManager.SelectedCharacterIndex);

        EditorApplication.ExitPlaymode();
#else
            Application.Quit();
#endif
    }
}