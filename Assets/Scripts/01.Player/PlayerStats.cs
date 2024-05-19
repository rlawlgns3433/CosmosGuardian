using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public PlayerShooter playerShooter;
    public TextMeshProUGUI textExp;
    public int level = 1;
    public int price = default;

    private readonly string scoreFormat = "Score : {0}";
    private float exp;
    [Tooltip("�ִ� ����")]
    public int maxLevel = 10;
    [Tooltip("�������� �ʿ��� ����ġ(��, ���ھ�)")]
    public float expForNextLevel = 100;
    [Tooltip("����ġ ����")]
    public float magnification = 1.8f;


    private CharacterTable characterTable = null;
    private Dictionary<CharacterColumn.Stat, float> initialStats = new Dictionary<CharacterColumn.Stat, float>(); // �ɷ�ġ ����, �ɷ�ġ ����
    public Dictionary<CharacterColumn.Stat, float> stats = new Dictionary<CharacterColumn.Stat, float>(); // �ɷ�ġ ����, �ɷ�ġ ����
    public List<ItemData> items = new List<ItemData>();
    private PrefabSelector prefabSelector = null;
    public CharacterData characterData = null;


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

        stats[CharacterColumn.Stat.HP] = initialStats[CharacterColumn.Stat.HP] = characterData.HP;
        stats[CharacterColumn.Stat.ARMOR] = initialStats[CharacterColumn.Stat.ARMOR] = characterData.ARMOR;
        stats[CharacterColumn.Stat.DAMAGE] = initialStats[CharacterColumn.Stat.DAMAGE] = characterData.DAMAGE_TYPE_1;
        stats[CharacterColumn.Stat.MOVE_SPEED_V] = initialStats[CharacterColumn.Stat.MOVE_SPEED_V] = characterData.MOVE_SPEED_V;
        stats[CharacterColumn.Stat.MOVE_SPEED_H] = initialStats[CharacterColumn.Stat.MOVE_SPEED_H] = characterData.MOVE_SPEED_H;
        stats[CharacterColumn.Stat.FIRE_RATE] = initialStats[CharacterColumn.Stat.FIRE_RATE] = characterData.FIRE_RATE;
        stats[CharacterColumn.Stat.FIRE_RANGE] = initialStats[CharacterColumn.Stat.FIRE_RANGE] = characterData.FIRE_RANGE;
        stats[CharacterColumn.Stat.PENETRATE] = initialStats[CharacterColumn.Stat.PENETRATE] = characterData.PENETRATE;
        stats[CharacterColumn.Stat.SPLASH_DAMAGE] = initialStats[CharacterColumn.Stat.SPLASH_DAMAGE] = characterData.SPLASH_DAMAGE;
        stats[CharacterColumn.Stat.SPLASH_RANGE] = initialStats[CharacterColumn.Stat.SPLASH_RANGE] = characterData.SPLASH_RANGE;
        stats[CharacterColumn.Stat.CRITICAL] = initialStats[CharacterColumn.Stat.CRITICAL] = characterData.CRITICAL;
        stats[CharacterColumn.Stat.CRITICAL_DAMAGE] = initialStats[CharacterColumn.Stat.CRITICAL_DAMAGE] = characterData.CRITICAL_DAMAGE;
        stats[CharacterColumn.Stat.HP_DRAIN] = initialStats[CharacterColumn.Stat.HP_DRAIN] = characterData.HP_DRAIN;
        stats[CharacterColumn.Stat.PROJECTILE_SPEED] = initialStats[CharacterColumn.Stat.PROJECTILE_SPEED] = characterData.PROJECTILE_SPEED;
        stats[CharacterColumn.Stat.PROJECTILE_AMOUNT] = initialStats[CharacterColumn.Stat.PROJECTILE_AMOUNT] = characterData.PROJECTILE_AMOUNT;

        price = characterData.PRICE;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            LevelUp();
        }
    }

    public bool UpdateStats(OptionColumn.Stat stat, OptionColumn.Type type, float value)
    {
        // ������ �������� stat�� observe�ϰ� �����ϴ� Ŭ������ �����Ͽ� ���� �ʿ�
        // Maximum�� ���� ��츦 ����� bool�� ��ȯ
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
                        // Weapon ���� ���׷��̵�
                        playerShooter.weapon.stats[(WeaponColumn.Stat)stat] += value;
                    }
                }
                break;
        }

        if(stat == OptionColumn.Stat.HP)
        {
            playerHealth.UpdateHealthUI();
        }

        return true;
    }

    public void GetExp(int score) // ���ھ exp�� ���
    {
        exp += score;
        textExp.text = string.Format(scoreFormat, exp);

        if (level >= maxLevel) return;

        if (exp > expForNextLevel)
        {
            LevelUp();
        }
    }

    public void LevelUp()
    {
        ++level;
        // level up ȿ�� ����
        expForNextLevel *= magnification;
        var platformIndex = GameManager.Instance.currentPlatformIndex;
        var platform = GameManager.Instance.platforms[platformIndex].GetComponent<Platform>();
        platform.optionController.ResetOptions(level);
    }
}
