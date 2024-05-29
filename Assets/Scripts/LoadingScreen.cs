using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public Slider progressBar;

    private void Start()
    {
        SceneIds sceneToLoad = ParamManager.SceneToLoad;

        switch(sceneToLoad)
        {
            case SceneIds.Main:
                if(ParamManager.currentRecord.score != -1)
                {
                    ParamManager.SaveCurrentRecord(ParamManager.currentRecord);
                }

                break;
        }

        StartCoroutine(LoadSceneAsync(sceneToLoad));
    }

    private IEnumerator LoadSceneAsync(SceneIds sceneToLoad)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync((int)sceneToLoad);

        while (!operation.isDone)
        {
            if (progressBar != null)
            {
                progressBar.value = Mathf.Clamp01(operation.progress / 0.9f);
            }

            yield return null;
        }
    }
}