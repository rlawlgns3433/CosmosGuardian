using UnityEngine;

public class OptionStat : MonoBehaviour
{
    public PlayerStats playerStats;
    public OptionColumn.Type type;
    public OptionColumn.Stat stat;
    public int value;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag(Tags.Player))
        {
            // playerStats�� ���� �� stat��
            // type (scale, fixed)�� ���� ����Ͽ�
            // value�� ����
            playerStats.UpdateStats(stat, type, value);
        }
    }
}
