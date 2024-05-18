using UnityEngine;

public class OptionStat : MonoBehaviour
{

    public PlayerStats playerStats;
    public OptionColumn.Type type;
    public OptionColumn.Stat stat;
    public float value;

    private UiController uiController;

    private void Start()
    {
        if(!GameObject.FindWithTag(Tags.UiController).TryGetComponent(out uiController))
        {
            uiController.enabled = false;
            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tags.Player))
        {
            // playerStats의 스텟 중 stat에
            // type (scale, fixed)의 양을 계산하여
            // value를 적용
            playerStats.UpdateStats(stat, type, value);
            gameObject.SetActive(false);

            var optionController = GetComponentInParent<OptionController>();
            foreach(var image in optionController.options)
            {
                Collider collider = image.GetComponent<Collider>();
                collider.enabled = false;
            }

            uiController.UpdatePauseUI();
        }
    }
}
