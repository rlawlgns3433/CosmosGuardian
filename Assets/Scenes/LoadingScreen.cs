using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Slider progressBar; // 프로그레스 바

    private void Start()
    {
        string sceneToLoad = PlayerPrefs.GetString("SceneToLoad");
        StartCoroutine(LoadSceneAsync(sceneToLoad));
    }

    private IEnumerator LoadSceneAsync(string sceneToLoad)
    {
        // 비동기 씬 로드 시작
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);

        // 씬 로드가 끝날 때까지 기다림
        while (!operation.isDone)
        {
            // 로딩 진행 상황 업데이트
            if (progressBar != null)
            {
                progressBar.value = Mathf.Clamp01(operation.progress / 0.9f);
            }

            yield return null;
        }
    }
}
