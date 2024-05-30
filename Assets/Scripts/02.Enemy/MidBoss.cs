using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MidBoss : Enemy
{
    protected override void Start()
    {
        var playerHealth = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerHealth>();

        onDeath += () =>
        {
            isAlive = false;
            StopAllCoroutines();
            var joystick = GameObject.FindWithTag(Tags.Joystick).GetComponent<FloatingJoystick>();
            joystick.gameObject.SetActive(false);
            PointerEventData eventData = new PointerEventData(EventSystem.current);
            joystick.OnPointerUp(eventData);

            Time.timeScale = 0f;
            var uiController = GameObject.FindWithTag(Tags.UiController).GetComponent<UiController>();
            uiController.item.SetActive(true);
            itemController = GameObject.FindWithTag(Tags.ItemController).GetComponent<ItemController>();
            itemController.UpdateItemData(enemyData.TYPE);
        };

        base.Start();
    }
}
