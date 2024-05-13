using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    public List<Image> options = new List<Image>(); // OnOff�� ����/�̻���
    public List<TextMeshProUGUI> optionTexts = new List<TextMeshProUGUI>();

    private OptionTable optionTable;

    private void OnEnable()
    {
        optionTable = DataTableMgr.Get<OptionTable>(DataTableIds.Option);
        // Ű �� �ϳ��� ����
        for (int i = 0; i < options.Count; ++i)
        {
            int index = Random.Range(0, optionTable.GetKeys.Count);

            optionTexts[i].text = optionTable.Get(optionTable.GetKeys[index]).NAME_DEV.ToString();

            OptionStat option = options[i].gameObject.GetComponent<OptionStat>();
            option.stat = optionTable.Get(optionTable.GetKeys[index]).STAT;
            option.type = optionTable.Get(optionTable.GetKeys[index]).TYPE;
            option.value = optionTable.Get(optionTable.GetKeys[index]).VALUE;
        }
    }
}
