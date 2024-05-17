using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public PlayerShooter playerShooter;
    public TextMeshProUGUI textExp;

    private readonly string scoreFormat = "Score : {0}";
    public int level = 1;
    private float exp;
    private float expForNextLevel = 100;

    private CharacterTable characterTable = null;
    private Dictionary<CharacterColumn.Stat, float> initialStats = new Dictionary<CharacterColumn.Stat, float>(); // 능력치 종류, 능력치 배율
    public Dictionary<CharacterColumn.Stat, float> stats = new Dictionary<CharacterColumn.Stat, float>(); // 능력치 종류, 능력치 배율
    private PrefabSelector prefabSelector = null;
    public CharacterData characterData = null;

    /// <summary>
    /// 시각적으로 스텟을 확인하기 위한 변수들
    /// </summary>

    public float DAMAGE;
    public float FIRE_RATE;
    public float FIRE_RANGE;
    public float PENETRATE;
    public float SPLASH_DAMAGE;
    public float SPLASH_RANGE;
    public float CRITICAL;
    public float CRITICAL_DAMAGE;
    public float HP_DRAIN;
    public float PROJECTILE_SPEED;
    public float PROJECTILE_AMOUNT;
    public float HP;
    public float MOVE_SPEED_V;
    public float MOVE_SPEED_H;
    public float ARMOR;

    // 이 부분을 Scriptable Assets로 변경할 수도 있음
    public int price = default;

    private void Awake()
    {
        characterTable = DataTableMgr.Get<CharacterTable>(DataTableIds.Character);
    }

    private void OnEnable()
    {
        if (!TryGetComponent(out prefabSelector))
        {
            prefabSelector.enabled = false;
            return;
        }

        StringBuilder sb = new StringBuilder();
        sb.Append((int)TableIdentifier.Character);
        sb.Append((prefabSelector.prefabNumber + 1).ToString("D2"));
        sb.Append((prefabSelector.prefabNumber + 1).ToString("D2")); // id + body(D2) + head(D2)
        string id = sb.ToString();
        characterData = characterTable.Get(int.Parse(id));

        HP = stats[CharacterColumn.Stat.HP] = initialStats[CharacterColumn.Stat.HP] = characterData.HP;
        ARMOR = stats[CharacterColumn.Stat.ARMOR] = initialStats[CharacterColumn.Stat.ARMOR] = characterData.ARMOR;
        DAMAGE = stats[CharacterColumn.Stat.DAMAGE] = initialStats[CharacterColumn.Stat.DAMAGE] = characterData.DAMAGE_TYPE_1;
        MOVE_SPEED_V = stats[CharacterColumn.Stat.MOVE_SPEED_V] = initialStats[CharacterColumn.Stat.MOVE_SPEED_V] = characterData.MOVE_SPEED_V;
        MOVE_SPEED_H = stats[CharacterColumn.Stat.MOVE_SPEED_H] = initialStats[CharacterColumn.Stat.MOVE_SPEED_H] = characterData.MOVE_SPEED_H;
        FIRE_RATE = stats[CharacterColumn.Stat.FIRE_RATE] = initialStats[CharacterColumn.Stat.FIRE_RATE] = characterData.FIRE_RATE;
        FIRE_RANGE = stats[CharacterColumn.Stat.FIRE_RANGE] = initialStats[CharacterColumn.Stat.FIRE_RANGE] = characterData.FIRE_RANGE;
        PENETRATE = stats[CharacterColumn.Stat.PENETRATE] = initialStats[CharacterColumn.Stat.PENETRATE] = characterData.PENETRATE;
        SPLASH_DAMAGE = stats[CharacterColumn.Stat.SPLASH_DAMAGE] = initialStats[CharacterColumn.Stat.SPLASH_DAMAGE] = characterData.SPLASH_DAMAGE;
        SPLASH_RANGE = stats[CharacterColumn.Stat.SPLASH_RANGE] = initialStats[CharacterColumn.Stat.SPLASH_RANGE] = characterData.SPLASH_RANGE;
        CRITICAL = stats[CharacterColumn.Stat.CRITICAL] = initialStats[CharacterColumn.Stat.CRITICAL] = characterData.CRITICAL;
        CRITICAL_DAMAGE = stats[CharacterColumn.Stat.CRITICAL_DAMAGE] = initialStats[CharacterColumn.Stat.CRITICAL_DAMAGE] = characterData.CRITICAL_DAMAGE;
        HP_DRAIN = stats[CharacterColumn.Stat.HP_DRAIN] = initialStats[CharacterColumn.Stat.HP_DRAIN] = characterData.HP_DRAIN;
        PROJECTILE_SPEED = stats[CharacterColumn.Stat.PROJECTILE_SPEED] = initialStats[CharacterColumn.Stat.PROJECTILE_SPEED] = characterData.PROJECTILE_SPEED;
        PROJECTILE_AMOUNT = stats[CharacterColumn.Stat.PROJECTILE_AMOUNT] = initialStats[CharacterColumn.Stat.PROJECTILE_AMOUNT] = characterData.PROJECTILE_AMOUNT;

        price = characterData.PRICE;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LevelUp();
        }
    }

    // 매개변수 : 스텟 종류, 증가량
    public bool UpdateStats(CharacterColumn.Stat stat, float value)
    {
        // Maximum이 있을 경우를 고려해 bool을 반환
        stats[stat] += value;

        return true;
    }

    public bool UpdateStats(OptionColumn.Stat stat, OptionColumn.Type type, float value)
    {
        // 옵저버 패턴으로 stat을 observe하고 동작하는 클래스를 생성하여 변경 필요
        // Maximum이 있을 경우를 고려해 bool을 반환
        switch (type)
        {
            case OptionColumn.Type.Scale:
                stats[(CharacterColumn.Stat)stat] += initialStats[(CharacterColumn.Stat)stat] * (value / 100.0f);
                break;
            case OptionColumn.Type.Fixed:
                {
                    if (stat == OptionColumn.Stat.HP)
                    {
                        stats[(CharacterColumn.Stat)stat] += value;
                        playerHealth.UpdateHealthUI();
                    }
                    else
                    {
                        // Weapon 값을 업그레이드
                        playerShooter.weapon.stats[(WeaponColumn.Stat)stat] += value;
                    }
                }
                break;
        }

        if(stat == OptionColumn.Stat.HP)
        {
            playerHealth.UpdateHealthUI();
        }

        SyncDevStat(); // 테스트
        return true;
    }

    public void GetExp(int score) // 스코어를 exp로 사용
    {
        exp += score;
        textExp.text = string.Format(scoreFormat, exp);
        if (exp > expForNextLevel)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        ++level;
        // level up 효과 생성
        expForNextLevel *= 1.8f;
        var platformIndex = GameManager.Instance.currentPlatformIndex;
        var platform = GameManager.Instance.platforms[platformIndex].GetComponent<Platform>();
        platform.optionController.ResetOptions(level);

    }

    public void SyncDevStat()
    {
        HP = stats[CharacterColumn.Stat.HP];
        ARMOR = stats[CharacterColumn.Stat.ARMOR];
        DAMAGE = stats[CharacterColumn.Stat.DAMAGE];
        MOVE_SPEED_V = stats[CharacterColumn.Stat.MOVE_SPEED_V];
        MOVE_SPEED_H = stats[CharacterColumn.Stat.MOVE_SPEED_H];
        FIRE_RATE = stats[CharacterColumn.Stat.FIRE_RATE];
        FIRE_RANGE = stats[CharacterColumn.Stat.FIRE_RANGE];
        PENETRATE = stats[CharacterColumn.Stat.PENETRATE];
        SPLASH_DAMAGE = stats[CharacterColumn.Stat.SPLASH_DAMAGE];
        SPLASH_RANGE = stats[CharacterColumn.Stat.SPLASH_RANGE];
        CRITICAL = stats[CharacterColumn.Stat.CRITICAL];
        CRITICAL_DAMAGE = stats[CharacterColumn.Stat.CRITICAL_DAMAGE];
        HP_DRAIN = stats[CharacterColumn.Stat.HP_DRAIN];
        PROJECTILE_SPEED = stats[CharacterColumn.Stat.PROJECTILE_SPEED];
        PROJECTILE_AMOUNT = stats[CharacterColumn.Stat.PROJECTILE_AMOUNT];
    }
}
