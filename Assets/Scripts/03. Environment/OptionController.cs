using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    public List<Image> options = new List<Image>(); // OnOff�� ����/�̻���
    public List<TextMeshProUGUI> optionTexts = new List<TextMeshProUGUI>();

    private OptionTable optionTable;

    private void Start()
    {
        optionTable = DataTableMgr.Get<OptionTable>(DataTableIds.Option);
    }

    private void Update()
    {
        // 2 ~ 3���� �������� Ȱ��ȭ
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Ű �� �ϳ��� ����

            foreach (var text in optionTexts)
            {
                int index = Random.Range(0, optionTable.GetKeys.Count);

                text.text = optionTable.Get(optionTable.GetKeys[index]).NAME_DEV.ToString();
                Debug.Log(text.text);
            }
        }
    }
}
