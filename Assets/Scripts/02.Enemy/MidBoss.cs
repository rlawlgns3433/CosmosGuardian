using UnityEngine;

public class MidBoss : Enemy
{
    protected override void Start()
    {
        var playerHealth = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerHealth>();

        onDeath += () =>
        {
            isAlive = false;
            if (target.playerStats.stats[CharacterColumn.Stat.HP] <= 0) return;

            Time.timeScale = 0f;
            var uiController = GameObject.FindWithTag(Tags.UiController).GetComponent<UiController>();
            uiController.item.SetActive(true);
            itemController = GameObject.FindWithTag(Tags.ItemController).GetComponent<ItemController>();
            itemController.UpdateItemData(enemyData.TYPE);
            GameObject.FindWithTag(Tags.Joystick).SetActive(false);
        };

        base.Start();
    }
}
