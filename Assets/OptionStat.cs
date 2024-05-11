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
            // playerStats의 스텟 중 stat에
            // type (scale, fixed)의 양을 계산하여
            // value를 적용
            playerStats.UpdateStats(stat, type, value);
        }
    }
}
