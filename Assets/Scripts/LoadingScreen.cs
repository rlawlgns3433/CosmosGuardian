using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Slider progressBar; // ���α׷��� ��

    private void Start()
    {
        string sceneToLoad = ParamManager.SceneToLoad;

        switch(sceneToLoad)
        {
            case "Main":
                ParamManager.SaveCurrentRecord(ParamManager.currentRecord);
                break;
        }

        StartCoroutine(LoadSceneAsync(sceneToLoad));
    }

    private IEnumerator LoadSceneAsync(string sceneToLoad)
    {
        // �񵿱� �� �ε� ����
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);

        // �� �ε尡 ���� ������ ��ٸ�
        while (!operation.isDone)
        {
            // �ε� ���� ��Ȳ ������Ʈ
            if (progressBar != null)
            {
                progressBar.value = Mathf.Clamp01(operation.progress / 0.9f);
            }

            yield return null;
        }
    }
}
