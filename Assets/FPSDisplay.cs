using UnityEngine;
using TMPro;
public class FPSDisplay : MonoBehaviour
{
    public TextMeshProUGUI fpsText;

    private void Update()
    {
        float currentFPS = 1.0f / Time.deltaTime;
        fpsText.text = "FPS: " + Mathf.Ceil(currentFPS).ToString();
    }
}
