using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

public class ScoreBoard : MonoBehaviour
{
    public static readonly string rankingFormat = "{0}, {1}, {2}점"; // 캐릭터 이름, 무기 이름, 점수

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

        //if (ParamManager.currentRecord == null)
        //{
        //    savedRecord.records.Add(ParamManager.currentRecord);
        //}
        //SaveLoadSystem.Save(savedRecord);

        for (int i = 0; i < savedRecord.records.Count; ++i)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(uiCharacterSelect.characterTable.Get(savedRecord.records[i].characterDataId).GetName);
            sb.Append(uiCharacterSelect.weaponTable.Get(savedRecord.records[i].weaponDataId).GetName);


            textRecords[i].text = string.Format(rankingFormat,
                uiCharacterSelect.characterTable.Get(savedRecord.records[i].characterDataId).GetName,
                uiCharacterSelect.weaponTable.Get(savedRecord.records[i].weaponDataId).GetName,
                savedRecord.records[i].score
                );
        }
    }
}
