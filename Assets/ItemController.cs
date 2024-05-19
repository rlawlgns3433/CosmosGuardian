using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class ItemController : MonoBehaviour
{
    private const int buttonCount = 3;
    private int startGrade = 1;

    public List<Button> buttons = new List<Button>();
    public List<Image> icons = new List<Image>();
    public List<TextMeshProUGUI> names = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> descs = new List<TextMeshProUGUI>();
    public PlayerStats playerStats;
    public List<ItemData> itemdatas = new List<ItemData>();

    private ItemTable itemTable;

    private void Start()
    {
        itemTable = DataTableMgr.Get<ItemTable>(DataTableIds.Item);
    }

    public void UpdateItemData()
    {
        itemdatas.Clear();

        // 전체 중 3개를 뽑아 -> 중복 없이
        List<int> id = new List<int>();
        List<int> grade = new List<int>();
        int itemCount = itemTable.GetKeys.ToList().Count;

        // 아이템 뽑기
        while(id.Count < 3)
        {
            int index = Random.Range(0, itemCount);
            int itemGrade = startGrade;
            int iconInt = itemTable.GetAllData[index].GetIconIdInt;

            if (id.Contains(iconInt))
            {
                continue;
            }
            // 만약 내가 찾은 iconInt와 같은 종류이면

            for (int k = 0; k < playerStats.items.Count; ++k)
            {
                if(playerStats.items[k].GetIconIdInt == itemTable.GetAllData[index].GetIconIdInt)
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

            foreach(var item in itemTable.GetAllData) 
            { 
                if(item.GetIconIdInt == iconInt && itemGrade == item.GRADE)
                {
                    itemdatas.Add(item);
                    break;
                }
            }
        }

        // 아이템 뿌려주기
        for(int i = 0; i < buttonCount; ++i )
        {
            icons[i].sprite = itemdatas[i].GetSprite;
            names[i].text = itemdatas[i].GetString;
            descs[i].text = itemdatas[i].GetString;

            List<OptionData> options = new List<OptionData>
            {
                itemdatas[i].GetOption1,
                itemdatas[i].GetOption2,
                itemdatas[i].GetOption3
            };

            var optionStat = buttons[i].GetComponent<OptionStat>();
            for(int j = 0; j < options.Count;++j)
            {
                optionStat.type.Add(options[j].TYPE);
                optionStat.stat.Add(options[j].STAT);
                optionStat.value.Add(options[j].VALUE);
            }

            buttons[i].GetComponent<ItemStat>().itemData = itemdatas[i];
        }
    }
}
