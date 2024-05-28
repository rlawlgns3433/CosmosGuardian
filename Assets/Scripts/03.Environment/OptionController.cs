using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

public class OptionController : MonoBehaviour
{
    private static readonly string Format = "<size=0.45>{0}\r\n<size=0.3>+{1}";
    private static readonly string WeaponChangeFormat = "<size=0.45>{0}";

    public List<Image> options = new List<Image>();
    public List<Image> bgImgs = new List<Image>();
    public List<TextMeshProUGUI> optionTexts = new List<TextMeshProUGUI>();
    public List<RawImage> rawImages = new List<RawImage>();
    public GameObject[] RenderTextureWeapons;

    private OptionTable optionTable;
    private PlayerStats playerStats;


    private void OnEnable()
    {
        playerStats = GameObject.FindWithTag(Tags.Player).GetComponent<PlayerStats>();
        optionTable = DataTableMgr.Get<OptionTable>(DataTableIds.Option);

        foreach (var option in options)
        {
            Collider collider = option.GetComponent<Collider>();
            collider.enabled = true;
        }

        ResetOptions(playerStats.level);
    }

    public void ResetOptions(int grade) // Grade 별로 옵션 생성
    {
        List<OptionData> gradedOptions = (from option in optionTable.GetAllData
                                          where option.GRADE == grade
                                          select option).ToList();

        //int selectedIndex = Random.Range(0, gradedOptions.Count);
        //gradedOptions = (from option in gradedOptions
        //                 where option.GRADE == gradedOptions[selectedIndex].GRADE
        //                 select option).ToList();

        List<int> selectedId = new List<int>();

        int excludeIndex = Random.Range(0, options.Count + 1); // Exclude 될 수도 있고 안 될 수도 있다. 그래서 배제할 인덱스를 1개 더 추가

        for (int i = 0; i < options.Count; ++i)
        {
            if (i == excludeIndex)
            {
                OptionStat excludedOption = options[excludeIndex].gameObject.GetComponent<OptionStat>();

                for (int j = 0; j < excludedOption.type.Count; ++j)
                {
                    if (excludedOption.type[j] == OptionColumn.Type.WeaponChange)
                    {
                        if(excludedOption.renderWeapon != null)
                        {
                            var cameraRenderTexture = excludedOption.renderWeapon.GetComponentInChildren<CameraRenderTexture>();
                            cameraRenderTexture.rawImage = null;
                            excludedOption.renderWeapon.SetActive(false);
                            excludedOption.renderWeapon = null;
                            excludedOption.rawImage.texture = null;
                            excludedOption.rawImage.gameObject.SetActive(false);
                            excludedOption.bgImg.gameObject.SetActive(false);
                        }
                    }
                }

                excludedOption.stat.Clear();
                excludedOption.type.Clear();
                excludedOption.value.Clear();

                options[i].gameObject.SetActive(false);
                continue;
            }

            int index = Random.Range(0, gradedOptions.Count);

            if (selectedId.Contains(gradedOptions[index].UPGRADE_ID))
            {
                --i;
                continue;
            }
            selectedId.Add(gradedOptions[index].UPGRADE_ID);

            options[i].gameObject.SetActive(true);

            Collider collider = options[i].GetComponent<Collider>();
            collider.enabled = true;

            switch (gradedOptions[index].TYPE)
            {
                case OptionColumn.Type.Scale:
                    optionTexts[i].text = string.Format(Format, gradedOptions[index].GetName, gradedOptions[index].VALUE.ToString()) + "%";

                    break;
                case OptionColumn.Type.Fixed:
                    optionTexts[i].text = string.Format(Format, gradedOptions[index].GetName, gradedOptions[index].VALUE.ToString());
                    break;
                case OptionColumn.Type.WeaponChange:
                    optionTexts[i].text = string.Format(WeaponChangeFormat, gradedOptions[index].GetName);
                    bgImgs[i].gameObject.SetActive(true);
                    break;
            }

            OptionStat option = options[i].gameObject.GetComponent<OptionStat>();

            for (int j = 0; j < option.type.Count; ++j)
            {
                if (option.type[j] == OptionColumn.Type.WeaponChange)
                {
                    if(option.renderWeapon != null)
                    {
                        var cameraRenderTexture = option.renderWeapon.GetComponentInChildren<CameraRenderTexture>();
                        cameraRenderTexture.rawImage = null;
                        option.renderWeapon.SetActive(false);
                        option.renderWeapon = null;
                        option.rawImage.texture = null;
                        option.rawImage.gameObject.SetActive(false);
                        bgImgs[i].gameObject.SetActive(false);
                    }
                }
            }

            option.stat.Clear();
            option.type.Clear();
            option.value.Clear();

            option.stat.Add(gradedOptions[index].STAT);
            option.type.Add(gradedOptions[index].TYPE);
            option.value.Add(gradedOptions[index].VALUE);

            if (gradedOptions[index].TYPE == OptionColumn.Type.WeaponChange)
            {
                foreach (var renderWeapon in RenderTextureWeapons)
                {
                    if (renderWeapon.activeInHierarchy) continue;

                    var cameraRenderWeapon = renderWeapon.GetComponentInChildren<CameraRenderWeapon>();
                    cameraRenderWeapon.SetWeapon(Mathf.RoundToInt(gradedOptions[index].VALUE) % 100);

                    option.rawImage.gameObject.SetActive(true);
                    option.renderWeapon = renderWeapon;

                    var cameraRenderTexture = option.renderWeapon.GetComponentInChildren<CameraRenderTexture>();
                    option.rawImage.texture = cameraRenderTexture.renderTexture;
                    cameraRenderTexture.rawImage = option.rawImage;
                    option.renderWeapon.SetActive(true);
                    break;
                }
            }
        }
    }
}
