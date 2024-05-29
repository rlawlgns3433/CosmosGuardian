using System.Collections.Generic;
using UnityEngine;

public class DynamicTextManager : MonoBehaviour
{
    public static DynamicTextData normalDamgeData;
    public static DynamicTextData criticalDamageData;
    public static DynamicTextData healingTextData;
    public static GameObject canvasPrefab;
    public static Transform mainCamera;

    public static List<GameObject> usingText = new List<GameObject>();
    public static List<GameObject> unusingText = new List<GameObject>();

    [SerializeField] private DynamicTextData _normalDamageData;
    [SerializeField] private DynamicTextData _criticalDamagData;
    [SerializeField] private DynamicTextData _healingTextData;
    [SerializeField] private GameObject _canvasPrefab;
    [SerializeField] private Transform _mainCamera;

    private void Awake()
    {
        normalDamgeData = _normalDamageData;
        criticalDamageData = _criticalDamagData;
        healingTextData = _healingTextData;
        mainCamera = _mainCamera;
        canvasPrefab = _canvasPrefab;
    }

    public static void CreateText2D(Vector2 position, string text, DynamicTextData data)
    {
        GameObject newText = Instantiate(canvasPrefab, position, Quaternion.identity);
        newText.transform.GetComponent<DynamicText2D>().Initialise(text, data);
    }

    public static void CreateText(Vector3 position, string text, DynamicTextData data)
    {
        GameObject newText;
        if (unusingText.Count <= 0)
        {
            newText = Instantiate(canvasPrefab, position, Quaternion.identity);
            usingText.Add(newText);
        }
        else
        {
            newText = GetText();
            newText.SetActive(true);
            newText.transform.position = position;
            newText.transform.rotation = Quaternion.identity;
        }
        newText.transform.GetComponent<DynamicText>().Initialise(text, data);
    }

    public static void ReturnText(GameObject text)
    {
        usingText.Remove(text);
        unusingText.Add(text);
        text.SetActive(false);
    }

    public static GameObject GetText()
    {
        var text = unusingText[0];
        usingText.Add(text);
        unusingText.Remove(text);

        return text;
    }
}