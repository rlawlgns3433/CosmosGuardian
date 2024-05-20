using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

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
        var optionController = GetComponentInParent<OptionController>();

        if (other.CompareTag(Tags.Player))
        {
            for(int i = 0; i < type.Count; ++i)
            {
                playerStats.UpdateStats(stat[i], type[i], value[i]);
                gameObject.SetActive(false);

                foreach (var image in optionController.options)
                {
                    Collider collider = image.GetComponent<Collider>();
                    collider.enabled = false;
                }
            }

            uiController.UpdatePauseUI();
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

        ClearOptions();
        Time.timeScale = 1f;
    }

    private void ClearOptions()
    {
        var itemController = GetComponentInParent<ItemController>();
        var optionStats = itemController.gameObject.GetComponentsInChildren<OptionStat>();

        foreach (var optionStat in optionStats)
        {
            optionStat.type.Clear();
            optionStat.stat.Clear();
            optionStat.value.Clear();
        }
    }
}
