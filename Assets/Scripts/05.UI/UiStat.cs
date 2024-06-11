using TMPro;
using UnityEngine;

public class UiStat : MonoBehaviour
{
    private readonly string nameFormat = "{0}Name";
    public CharacterColumn.Stat stat;
    public TextMeshProUGUI textStat;

    private void Start()
    {
        textStat.text = DataTableMgr.GetStringTable().Get(string.Format(nameFormat, (int)stat));
    }
}
