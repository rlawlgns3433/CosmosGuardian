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
        // ���İ��� ��ȭ��Ű�� ���� TextMeshProUGUI�� ������ ����
        Color originalColor = textIntro.color;
        float timer = 0;

        while (true)
        {
            // Ÿ�̸Ӱ� �����ϸ鼭 ���� ���� ���
            timer += Time.deltaTime;
            float alpha = Mathf.PingPong(timer / blinkDuration, 1.0f);

            // ���� ���� ����
            textIntro.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            yield return null; // �����Ӹ��� ������Ʈ
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
