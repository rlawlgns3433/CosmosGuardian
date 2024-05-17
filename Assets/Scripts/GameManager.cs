using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public List<GameObject> platforms = new List<GameObject>();
    public List<GameObject> enemies = new List<GameObject>(); // GameManager에서 갖는 것이 좋겠음.

    public int currentPlatformIndex = 0;
    public int nextPlatformIndex = 1;
    public EnemyTable enemyTable;
    public PlayerStats playerStats = null;
    public TextMeshProUGUI textPasue;
    public bool IsGameover { get; set; }
    public bool IsPaused { get; set; }

    public float platformSpacing = 27f;

    private void Awake()
    {
        enemyTable = DataTableMgr.Get<EnemyTable>(DataTableIds.Enemy);
    }

    //private void Start()
    //{
    //    GameObject currentPlatform = platforms[currentPlatformIndex];
    //    var platform = currentPlatform.GetComponent<Platform>();

    //    foreach (var enemy in platform.spawnedEnemies)
    //    {
    //        enemy.Chase();
    //    }
    //}

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

            // 다음 플랫폼의 몬스터들이 움직이게 함
            //Platform nextPlatform = platforms[nextPlatformIndex].GetComponent<Platform>();
            //foreach(var enemy in nextPlatform.spawnedEnemies)
            //{
            //    enemy.Chase();
            //}
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            if(IsGameover)
            {
                SceneManager.LoadScene(0);
            }
        }
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!IsPaused)
            {
                Time.timeScale = 0f;
            }
            else
            {
                Time.timeScale = 1f;
            }
            IsPaused = !IsPaused;
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

        foreach(var collider in colliders)
        {
            collider.enabled = false;
        }
    }

    public void Pasue()
    {
        if (!IsPaused)
        {
            textPasue.gameObject.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            textPasue.gameObject.SetActive(false);
            Time.timeScale = 1f;
        }
        IsPaused = !IsPaused;
    }
}
