using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using Unity.VisualScripting;

public class OptionController : MonoBehaviour
{
    public List<Image> options = new List<Image>(); // OnOff로 생성/미생성
    public List<TextMeshProUGUI> optionTexts = new List<TextMeshProUGUI>();

    private OptionTable optionTable;
    private PlayerStats playerStats;


    private void OnEnable()
    {
        playerStats = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerStats>();
        optionTable = DataTableMgr.Get<OptionTable>(DataTableIds.Option);
        // 키 중 하나를 선택
        ResetOptions(playerStats.level);
    }

    public void ResetOptions(int grade) // Grade 별로 옵션 생성
    {
        List<OptionData> gradedOptions = (from option in optionTable.GetAllData
                                          where option.GRADE <= grade
                                          select option).ToList();

        List<int> selectedId = new List<int>();

        int excludeIndex = Random.Range(0, options.Count + 1); // Exclude 될 수도 있고 안 될 수도 있다. 그래서 배제할 인덱스를 1개 더 추가

        for (int i = 0; i < options.Count; ++i)
        {
            if (i == excludeIndex)
            {
                options[i].gameObject.SetActive(false);
                continue;
            }

                int index = Random.Range(0, gradedOptions.Count);

            if (selectedId.Contains(gradedOptions[index].UPGRADE_ID))
            {
                --i;
                continue;
            }
            selectedId.Add(gradedOptions[index].UPGRADE_ID);

            options[i].gameObject.SetActive(true);
            optionTexts[i].text = gradedOptions[index].NAME_DEV.ToString();
            OptionStat option = options[i].gameObject.GetComponent<OptionStat>();
            option.stat = gradedOptions[index].STAT;
            option.type = gradedOptions[index].TYPE;
            option.value = gradedOptions[index].VALUE;
        }
    }
}
