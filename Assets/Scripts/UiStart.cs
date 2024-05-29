using System.Collections;
using TMPro;
using UnityEngine;

public class UiStart : MonoBehaviour
{
    public AudioSource audioSource;
    public TextMeshProUGUI textIntro;
    public float blinkDuration = 1.0f;

    private void Start()
    {
        StartCoroutine(BlinkText());
    }

    private IEnumerator BlinkText()
    {
        Color originalColor = textIntro.color;
        float timer = 0;

        while (true)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.PingPong(timer / blinkDuration, 1.0f);
            textIntro.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            yield return null;
        }
    }

    public void EnterMainScene()
    {
        ParamManager.SceneToLoad = SceneIds.Main;
        ParamManager.LoadScene(ParamManager.SceneToLoad);

        StopAllCoroutines();
        audioSource.Stop();
        Destroy(audioSource);
        Destroy(this);
    }
}
