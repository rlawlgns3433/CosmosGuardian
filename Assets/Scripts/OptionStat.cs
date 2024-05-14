using UnityEngine;

public class OptionStat : MonoBehaviour
{
    public PlayerStats playerStats;
    public OptionColumn.Type type;
    public OptionColumn.Stat stat;
    public float value;

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
            

            // 부모 컴포넌트 중 optioncontroller를 찾고
            // option (Image)들을 검색하고 이들의 모든 collider를 끈다.
            // 플랫폼이 생성될 때 active를 설정하고
            // collider를 켠다.
        }
    }
}
