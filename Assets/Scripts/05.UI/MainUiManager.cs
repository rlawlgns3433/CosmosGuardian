using UnityEngine;
using UnityEngine.SceneManagement;

public class MainUiManager : MonoBehaviour
{
    public void EnterGame()
    {
        SceneManager.LoadScene(1);
    }

    public void EnterShop()
    {
        SceneManager.LoadScene(2);
    }
}
