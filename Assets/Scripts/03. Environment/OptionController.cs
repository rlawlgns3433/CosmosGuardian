using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    // ���� �÷����� ���� �÷����� �����ؾ� ��
    // 

    public List<Image> options = new List<Image>(); // OnOff�� ����/�̻���
    public List<TextMeshProUGUI> optionTexts = new List<TextMeshProUGUI>();

    private OptionTable optionTable;

    private void OnEnable()
    {
        optionTable = DataTableMgr.Get<OptionTable>(DataTableIds.Option);

        // Ű �� �ϳ��� ����
        foreach (var text in optionTexts)
        {
            int index = Random.Range(0, optionTable.GetKeys.Count);

            text.text = optionTable.Get(optionTable.GetKeys[index]).NAME_DEV.ToString();
        }
    }
}
