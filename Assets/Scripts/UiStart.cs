using System.Collections;
using System.Collections.Generic;
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
        // 알파값을 변화시키기 위해 TextMeshProUGUI의 색상을 변경
        Color originalColor = textIntro.color;
        float timer = 0;

        while (true)
        {
            // 타이머가 증가하면서 알파 값을 계산
            timer += Time.deltaTime;
            float alpha = Mathf.PingPong(timer / blinkDuration, 1.0f);

            // 알파 값을 설정
            textIntro.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            yield return null; // 프레임마다 업데이트
        }
    }

    public void EnterMainScene()
    {
        ParamManager.SceneToLoad = "Main";
        ParamManager.LoadScene(ParamManager.SceneToLoad);

        StopAllCoroutines();
        audioSource.Stop();
        Destroy(audioSource);
        Destroy(this);
    }
}
