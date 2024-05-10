using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionController : MonoBehaviour
{
    public List<Image> options = new List<Image>(); // OnOff로 생성/미생성
    public List<TextMeshProUGUI> optionTexts = new List<TextMeshProUGUI>();

    private OptionTable optionTable;

    private void Start()
    {
        optionTable = DataTableMgr.Get<OptionTable>(DataTableIds.Option);
    }

    private void Update()
    {
        // 2 ~ 3개의 선택지만 활성화
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // 키 중 하나를 선택

            foreach (var text in optionTexts)
            {
                int index = Random.Range(0, optionTable.GetKeys.Count);

                text.text = optionTable.Get(optionTable.GetKeys[index]).NAME_DEV.ToString();
                Debug.Log(text.text);
            }
        }
    }
}
