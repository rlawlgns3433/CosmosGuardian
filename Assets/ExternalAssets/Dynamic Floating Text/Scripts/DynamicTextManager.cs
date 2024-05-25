using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DynamicTextManager : MonoBehaviour
{
    public static DynamicTextData normalDamgeData;
    public static DynamicTextData criticalDamageData;
    public static GameObject canvasPrefab;
    public static Transform mainCamera;

    [SerializeField] private DynamicTextData _normalDamageData;
    [SerializeField] private DynamicTextData _criticalDamagData;
    [SerializeField] private GameObject _canvasPrefab;
    [SerializeField] private Transform _mainCamera;

    private void Awake()
    {
        normalDamgeData = _normalDamageData;
        criticalDamageData = _criticalDamagData;
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
        GameObject newText = Instantiate(canvasPrefab, position, Quaternion.identity);
        newText.transform.GetComponent<DynamicText>().Initialise(text, data);
    }

}
