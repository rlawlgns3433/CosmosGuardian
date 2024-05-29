using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Text;

public class ItemController : MonoBehaviour
{
    private static readonly string redText = "<color=red>";
    private static readonly string greenText = "<color=green>";
    private static readonly string colorEndText = "</color>";
    private const int buttonCount = 3;
    private int startGrade = 1;

    public List<Button> buttons = new List<Button>();
    public List<Image> icons = new List<Image>();
    public List<TextMeshProUGUI> names = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> descs = new List<TextMeshProUGUI>();
    public List<ItemData> itemdatas = new List<ItemData>();
    public PlayerStats playerStats;
    private ItemTable itemTable;

    private void Awake()
    {
        itemTable = DataTableMgr.Get<ItemTable>(DataTableIds.Item);
    }

    public void UpdateItemData(int type)
    {
        itemdatas.Clear();

        List<int> id = new List<int>();
        List<int> grade = new List<int>();
        int itemCount = itemTable.GetKeys.ToList().Count;

        while (id.Count < 3)
        {
            int index = Random.Range(0, itemCount);
            int itemGrade = startGrade;
            int iconInt = itemTable.GetAllData[index].GetIconIdInt;

            if (itemTable.GetAllData[index].TYPE != type) continue;

            if (id.Contains(iconInt))
            {
                continue;
            }
            // 만약 내가 찾은 iconInt와 같은 종류이면
            for (int k = 0; k < playerStats.items.Count; ++k)
            {
                if (playerStats.items[k].GetIconIdInt == itemTable.GetAllData[index].GetIconIdInt)
                {
                    ItemData playerData = new ItemData();

                    playerData = playerStats.items[k];

                    if (playerData.GRADE >= 3)
                    {
                        continue;
                    }
                    else
                    {
                        itemGrade = playerData.GRADE + 1;
                    }
                }
            }

            grade.Add(itemGrade);

            id.Add(iconInt);

            foreach (var item in itemTable.GetAllData)
            {
                if (item.GetIconIdInt == iconInt && itemGrade == item.GRADE)
                {
                    itemdatas.Add(item);
                    break;
                }
            }
        }

        // 아이템 뿌려주기
        for (int i = 0; i < buttonCount; ++i)
        {
            StringBuilder sb = new StringBuilder();

            icons[i].sprite = itemdatas[i].GetSprite;
            names[i].text = itemdatas[i].GetString;

            sb.Append(itemdatas[i].GetOption1.GetName);
            sb.Append(" ");
            sb.Append(FormatSignedValue(itemdatas[i].GetOption1));
            sb.Append("\n");

            if (itemdatas[i].GetOption2 != null)
            {
                sb.Append(itemdatas[i].GetOption2.GetName);
                sb.Append(" ");
                sb.Append(FormatSignedValue(itemdatas[i].GetOption2));
                sb.Append("\n");
            }

            if(itemdatas[i].GetOption3 != null)
            {
                sb.Append(itemdatas[i].GetOption3.GetName);
                sb.Append(" ");
                sb.Append(FormatSignedValue(itemdatas[i].GetOption3));
            }

            descs[i].text = sb.ToString();

            List<OptionData> options = new List<OptionData>
            {
                itemdatas[i].GetOption1
            };

            if (itemdatas[i].GetOption2 != default(OptionData))
            {
                options.Add(itemdatas[i].GetOption2);
            }

            if (itemdatas[i].GetOption3 != default(OptionData))
            {
                options.Add(itemdatas[i].GetOption3);
            }

            var optionStat = buttons[i].GetComponent<OptionStat>();
            for (int j = 0; j < options.Count; ++j)
            {
                optionStat.type.Add(options[j].TYPE);
                optionStat.stat.Add(options[j].STAT);
                optionStat.value.Add(options[j].VALUE);
            }

            buttons[i].GetComponent<ItemStat>().itemData = itemdatas[i];
        }
    }

    public static string FormatSignedValue(OptionData optionData)
    {
        StringBuilder sb = new StringBuilder();

        if (optionData.VALUE < 0)
        {
            sb.Append(redText);
        }
        else
        {
            sb.Append(greenText);
        }
        sb.Append(optionData.VALUE.ToString("+#.##;-#.##;+0.00"));

        if(optionData.TYPE == OptionColumn.Type.Scale)
        {
            sb.Append("%");
        }

        sb.Append(colorEndText);

        return sb.ToString();
    }
}