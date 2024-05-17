using UnityEngine;

public class OptionStat : MonoBehaviour
{
    UiController uiController;

    public PlayerStats playerStats;
    public OptionColumn.Type type;
    public OptionColumn.Stat stat;
    public float value;

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
            // playerStats�� ���� �� stat��
            // type (scale, fixed)�� ���� ����Ͽ�
            // value�� ����
            playerStats.UpdateStats(stat, type, value);
            gameObject.SetActive(false);

            var optionController = GetComponentInParent<OptionController>();
            foreach(var image in optionController.options)
            {
                Collider collider = image.GetComponent<Collider>();
                collider.enabled = false;
            }

            uiController.UpdatePauseUI();

            // �θ� ������Ʈ �� optioncontroller�� ã��
            // option (Image)���� �˻��ϰ� �̵��� ��� collider�� ����.
            // �÷����� ������ �� active�� �����ϰ�
            // collider�� �Ҵ�.
        }
    }
}
