using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    // 현재 플랫폼과 다음 플랫폼을 관리해야 함
    // 

    public List<Image> options = new List<Image>(); // OnOff로 생성/미생성
    public List<TextMeshProUGUI> optionTexts = new List<TextMeshProUGUI>();

    private OptionTable optionTable;

    private void OnEnable()
    {
        optionTable = DataTableMgr.Get<OptionTable>(DataTableIds.Option);

        // 키 중 하나를 선택
        foreach (var text in optionTexts)
        {
            int index = Random.Range(0, optionTable.GetKeys.Count);

            text.text = optionTable.Get(optionTable.GetKeys[index]).NAME_DEV.ToString();
        }
    }
}
