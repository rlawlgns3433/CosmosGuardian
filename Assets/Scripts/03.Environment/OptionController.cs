using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class OptionController : MonoBehaviour
{
    private static readonly string Format = "<size=0.45>{0}\r\n<size=0.3>+{1}";
    private static readonly string[] optionName =
    {
        "데미지",
        "공격 빈도",
        "사거리",
        "관통",
        "스플래시 데미지",
        "스플래시 범위",
        "치명타 확률",
        "치명타 데미지",
        "HP 흡수",
        "투사체 속도",
        "투사체 개수",
        "HP",
        "",
        "좌우 속도",
        "방어구"
    };


    public List<Image> options = new List<Image>();
    public List<TextMeshProUGUI> optionTexts = new List<TextMeshProUGUI>();

    private OptionTable optionTable;
    private PlayerStats playerStats;


    private void OnEnable()
    {
        playerStats = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerStats>();
        optionTable = DataTableMgr.Get<OptionTable>(DataTableIds.Option);

        foreach(var option in options)
        {
            Collider collider = option.GetComponent<Collider>();
            collider.enabled = true;
        }

        ResetOptions(playerStats.level);
    }

    public void ResetOptions(int grade) // Grade 별로 옵션 생성
    {
        List<OptionData> gradedOptions = (from option in optionTable.GetAllData
                                          where option.GRADE <= grade
                                          select option).ToList();

        int selectedIndex = Random.Range(0, gradedOptions.Count);
        gradedOptions = (from option in gradedOptions
                         where option.GRADE == gradedOptions[selectedIndex].GRADE
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

            Collider collider = options[i].GetComponent<Collider>();
            collider.enabled = true;

            //optionTexts[i].text = gradedOptions[index].NAME_DEV.ToString();

            switch (gradedOptions[index].TYPE)
            {
                case OptionColumn.Type.Scale:
                    optionTexts[i].text = string.Format(Format, optionName[(int)gradedOptions[index].STAT].ToString(), gradedOptions[index].VALUE.ToString()) + "%";

                    break;
                case OptionColumn.Type.Fixed:
                    optionTexts[i].text = string.Format(Format, optionName[(int)gradedOptions[index].STAT].ToString(), gradedOptions[index].VALUE.ToString());
                    break;
            }

            OptionStat option = options[i].gameObject.GetComponent<OptionStat>();
            option.stat = gradedOptions[index].STAT;
            option.type = gradedOptions[index].TYPE;
            option.value = gradedOptions[index].VALUE;
        }
    }
}
