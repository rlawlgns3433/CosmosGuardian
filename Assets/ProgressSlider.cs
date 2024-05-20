using UnityEngine;
using UnityEngine.UI;

public class ProgressSlider : MonoBehaviour
{
    public Slider progressSlider;
    public PlayerMovement playerMovement;
    // PlayerMovement에서 플레이어 위치의 z축
    // GameManager에서 플랫폼의 수를 가져옴  pooled platform count = 8
    // stop position when encountered boss monster = 15
    // platform distance = 27
    // BossSpawnController의 bossSpawnThreshold = 3

    private int stage = 1;
    private int startPlatformDistance = 27;
    private int pooledPlatformCount;
    private int stopDistanceAtBoss = 15;
    private int platformDistance = 27;
    private int totalDistance = 0;
    private int appearanceThreshold;
    private float progressRate = 0f;
    private float movedDistance = 0f;
    private float stageLength = 0f;
    private int distanceToNextStage = 12;

    public BossSpawnController bossSpawnController;

    private void Start()
    {
        pooledPlatformCount = GameManager.Instance.platforms.Count;
        appearanceThreshold = bossSpawnController.bossAppearanceThreshold;
        totalDistance = startPlatformDistance + (appearanceThreshold + 1) * platformDistance * pooledPlatformCount + stopDistanceAtBoss;
        progressRate = playerMovement.transform.position.z / totalDistance;
    }

    private void Update()
    {
        if (Time.frameCount % 3 == 0)
        {
            if(stage == 1)
            {
                movedDistance = playerMovement.transform.position.z;
            }
            else
            {
                movedDistance = playerMovement.transform.position.z - stageLength;
            }
            progressRate = movedDistance / totalDistance;
            progressSlider.value = progressRate;
        }
    }

    // 스테이지 클리어 시 호출 필요
    public void ResetProgressSlider()
    {
        ++stage;
        // 플레이어 위치에서 totalDistance 뺀 값을 설정
        stageLength += movedDistance;
        movedDistance = 0; // 플레이어가 이동한 거리
        totalDistance = distanceToNextStage + (appearanceThreshold + 1) * platformDistance * (pooledPlatformCount - 1)+ stopDistanceAtBoss; // 다음 이동할 거리
    }
}
