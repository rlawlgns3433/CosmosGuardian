using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.UI;

public class ProgressSlider : MonoBehaviour
{
    public Slider progressSlider;
    public PlayerMovement playerMovement;
    // PlayerMovement���� �÷��̾� ��ġ�� z��
    // GameManager���� �÷����� ���� ������  pooled platform count = 8
    // stop position when encountered boss monster = 15
    // platform distance = 27
    // BossSpawnController�� bossSpawnThreshold = 3

    private int startPlatformDistance = 27;
    private int pooledPlatformCount;
    private int stopDistanceAtBoss = 12;
    private int platformDistance = 27;
    private int totalDistance = 0;
    private int appearanceThreshold;
    private float progressRate = 0f;

    public BossSpawnController bossSpawnController;

    private void Start()
    {
        pooledPlatformCount = GameManager.Instance.platforms.Count;
        appearanceThreshold = bossSpawnController.bossAppearanceThreshold;
        totalDistance = startPlatformDistance + (appearanceThreshold + 1) * platformDistance * pooledPlatformCount + stopDistanceAtBoss;
        progressRate = playerMovement.transform.position.z / totalDistance;

        Debug.Log(progressRate);
    }

    private void Update()
    {
        if(Time.frameCount % 3 == 0)
        {
            progressRate = playerMovement.transform.position.z / totalDistance;
            progressSlider.value = progressRate;
        }
    }
}
