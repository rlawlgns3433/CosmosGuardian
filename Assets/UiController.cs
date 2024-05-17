using UnityEngine;

public class UiController : MonoBehaviour
{
    [Header("Group")]
    public GameObject group;
    public GameObject pause;

    private void Start()
    {
        group.SetActive(true);
        pause.SetActive(false);
    }
}
