using UnityEngine;
using TMPro;
using System.Text;
using System.Collections.Generic;

public class UiCharacterDesc : MonoBehaviour
{
    public UiCharacterSelect uiCharacterSelect;
    public TextMeshProUGUI textWindowHeader;
    public TextMeshProUGUI textCharacterDesc;

    private Dictionary<int, CharacterData> data = new Dictionary<int, CharacterData>();


    // uiCharacterSelect character index로 desc를 설정
    private void Awake()
    {
        data = uiCharacterSelect.characterTable.AllData;
    }

    private void OnEnable()
    {
        if(data == null)
        {
            data = uiCharacterSelect.characterTable.AllData;
        }

        StringBuilder sb = new StringBuilder();
        sb.Append((int)TableIdentifier.Character);
        sb.Append((uiCharacterSelect.selectedCharacterIndex + 1).ToString("D2"));
        sb.Append((uiCharacterSelect.selectedCharacterIndex + 1).ToString("D2"));
        string id = sb.ToString();

        int intId = int.Parse(id);
        textCharacterDesc.text = data[intId].GetDesc;
        textWindowHeader.text = data[intId].GetName;
    }
}
