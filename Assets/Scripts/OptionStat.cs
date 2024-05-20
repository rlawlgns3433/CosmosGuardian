using System.Collections.Generic;
using UnityEngine;

public class OptionStat : MonoBehaviour
{
    public PlayerStats playerStats;
    public List<OptionColumn.Type> type = new List<OptionColumn.Type>();
    public List<OptionColumn.Stat> stat = new List<OptionColumn.Stat>();
    public List<float> value = new List<float>();

    private UiController uiController;

    private void Start()
    {
        if(!GameObject.FindWithTag(Tags.UiController).TryGetComponent(out uiController))
        {
            uiController.enabled = false;
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tags.Player))
        {
            for(int i = 0; i < type.Count; ++i)
            {
                playerStats.UpdateStats(stat[i], type[i], value[i]);
                gameObject.SetActive(false);

                var optionController = GetComponentInParent<OptionController>();
                foreach (var image in optionController.options)
                {
                    Collider collider = image.GetComponent<Collider>();
                    collider.enabled = false;
                }
            }

            uiController.UpdatePauseUI();

            type.Clear();
            stat.Clear();
            value.Clear();
        }
    }

    public void OptionButtonClicked()
    {
        for (int i = 0; i < type.Count; ++i)
        {
            playerStats.UpdateStats(stat[i], type[i], value[i]);

            uiController.UpdatePauseUI();
        }

        playerStats.items.Add(GetComponent<ItemStat>().itemData);

        // sibling node에 있는 optionstat 초기화 필요
        var itemController = GetComponentInParent<ItemController>(); // null
        var optionStats = itemController.gameObject.GetComponentsInChildren<OptionStat>();

        foreach(var optionStat in optionStats)
        { 
            optionStat.type.Clear();
            optionStat.stat.Clear();  
            optionStat.value.Clear();  
        }
        Time.timeScale = 1f;
    }
}
