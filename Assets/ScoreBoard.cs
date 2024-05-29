using System.Text;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public static readonly string rankingFormat = "{0}Á¡, {1}, {2}";
    public TextMeshProUGUI[] textRecords;
    public SaveDataV1 savedRecord;
    public UiCharacterSelect uiCharacterSelect;

    private void OnEnable()
    {
        savedRecord = SaveLoadSystem.Load() as SaveDataV1;

        if (savedRecord == null)
        {
            return;
        }

        for (int i = 0; i < savedRecord.records.Count; ++i)
        {
            if (savedRecord.records[i].score <= 0) continue;

            StringBuilder sb = new StringBuilder();
            sb.Append(uiCharacterSelect.characterTable.Get(savedRecord.records[i].characterDataId).GetName);
            sb.Append(uiCharacterSelect.weaponTable.Get(savedRecord.records[i].weaponDataId).GetName);


            textRecords[i].text = string.Format(rankingFormat,
                savedRecord.records[i].score,
                uiCharacterSelect.characterTable.Get(savedRecord.records[i].characterDataId).GetName,
                uiCharacterSelect.weaponTable.Get(savedRecord.records[i].weaponDataId).GetName
                );
        }
    }
}
