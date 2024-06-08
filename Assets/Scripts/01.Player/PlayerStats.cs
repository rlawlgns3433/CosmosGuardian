using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using System.Collections;

public class PlayerWeaponData
{
    public OptionColumn.Stat playerStat;
    public float playerValue;

    public override string ToString()
    {
        return $"playerStat : {playerStat}, playerValue : {playerValue}\n";
    }
}

public class PlayerStats : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public PlayerShooter playerShooter;
    public TextMeshProUGUI textExp;
    public GameObject getOptionEffect;
    private ParticleSystem[] effects;
    private WaitForSeconds twoSec = new WaitForSeconds(2f);
    private Coroutine stopEffectCoroutine;
    public int level = 1;

    private readonly string scoreFormat = "Score : {0}";
    public int exp;
    [Tooltip("최대 레벨")]
    public int maxLevel = 10;
    [Tooltip("레벨업에 필요한 경험치(즉, 스코어)")]
    public float expForNextLevel = 100;
    [Tooltip("경험치 배율")]
    public float magnification = 1.8f;

    private CharacterTable characterTable;
    public CharacterStat standardStats = new CharacterStat();
    public CharacterStat initialStats = new CharacterStat();
    public CharacterStat stats = new CharacterStat();
    public List<ItemData> items = new List<ItemData>();
    public List<PlayerWeaponData> playerWeaponDatas = new List<PlayerWeaponData>();
    public PrefabSelector prefabSelector = null;
    public CharacterData characterData = null;
    public CharacterData standardCharacterData = null;
    public string id = string.Empty;
    public float prevPositionZ = 1.5f;

    private void Awake()
    {
        characterTable = DataTableMgr.Get<CharacterTable>(DataTableIds.Character);
        effects = getOptionEffect.GetComponentsInChildren<ParticleSystem>();
        prevPositionZ = transform.position.z;
    }

    private void OnEnable()
    {
        if (characterData == null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((int)TableIdentifier.Character);
            sb.Append((prefabSelector.SelectedCharacterIndex + 1).ToString("D2"));
            sb.Append((prefabSelector.SelectedCharacterIndex + 1).ToString("D2"));
            id = sb.ToString();

            InitCharacterInfo(id);
        }
    }

    private void LateUpdate()
    {
        if ((int)transform.position.z > (int)prevPositionZ)
        {
            GetExp(1);
            prevPositionZ = transform.position.z;
        }
    }

    public void UpdateStats(OptionColumn.Stat stat, OptionColumn.Type type, float value)
    {
        if (type != OptionColumn.Type.ApplyChangeWeaponData && stat != OptionColumn.Stat.MOVE_SPEED_V)
        {
            ActivateOptionEffect();
        }

        switch (type)
        {
            case OptionColumn.Type.Scale:
                ApplyScaleStat(stat, value);
                break;
            case OptionColumn.Type.Fixed:
                ApplyFixedStat(stat, value);
                break;
            case OptionColumn.Type.WeaponChange:
                playerShooter.weapon.SetWeapon(Mathf.RoundToInt(value));
                break;
            case OptionColumn.Type.ApplyChangeWeaponData:
                ApplyChangeWeaponData(stat, value);
                break;
        }

        if (stopEffectCoroutine != null)
        {
            StopCoroutine(stopEffectCoroutine);
        }

        stopEffectCoroutine = StartCoroutine(StopEffectAfter(twoSec));
    }

    private void ApplyScaleStat(OptionColumn.Stat stat, float value)
    {
        if (stat == OptionColumn.Stat.HP)
        {
            stats.stat[(CharacterColumn.Stat)stat] += stats.stat[(CharacterColumn.Stat)stat] * (value / 100.0f);
            playerHealth.UpdateHealthUI();
        }
        else
        {
            stats.stat[(CharacterColumn.Stat)stat] += initialStats.stat[(CharacterColumn.Stat)stat] * (value / 100.0f);
        }

        ConstraintCharacterAbility(stat);
    }

    private void ApplyFixedStat(OptionColumn.Stat stat, float value)
    {
        if (stat == OptionColumn.Stat.HP)
        {
            stats.stat[(CharacterColumn.Stat)stat] += value;
            playerHealth.UpdateHealthUI();
        }
        else
        {
            playerShooter.weapon.stats[(WeaponColumn.Stat)stat] += value;
            playerWeaponDatas.Add(new PlayerWeaponData { playerStat = stat, playerValue = value });
            ConstraintWeaponAbility(stat);
        }
    }

    private void ApplyChangeWeaponData(OptionColumn.Stat stat, float value)
    {
        playerShooter.weapon.stats[(WeaponColumn.Stat)stat] += value;
        ConstraintWeaponAbility(stat);
    }

    private void ActivateOptionEffect()
    {
        getOptionEffect.SetActive(true);
        foreach (var particle in effects)
        {
            particle.Play();
        }
    }

    public void GetExp(int score)
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
        expForNextLevel *= magnification;
        var platform = GameManager.Instance.platforms[GameManager.Instance.currentPlatformIndex].GetComponent<Platform>();
        platform.optionController.ResetOptions(level);
    }

    public void InitCharacterInfo(string id)
    {
        characterData = characterTable.Get(int.Parse(id));
        standardCharacterData = characterTable.Get(20101);

        InitializeStats(characterData, initialStats.stat, stats.stat);
        InitializeStats(standardCharacterData, standardStats.stat);
    }

    private void InitializeStats(CharacterData data, Dictionary<CharacterColumn.Stat, float> targetDict)
    {
        targetDict[CharacterColumn.Stat.HP] = data.HP;
        targetDict[CharacterColumn.Stat.ARMOR] = data.ARMOR;
        targetDict[CharacterColumn.Stat.DAMAGE] = data.DAMAGE_TYPE_1;
        targetDict[CharacterColumn.Stat.MOVE_SPEED_V] = data.MOVE_SPEED_V;
        targetDict[CharacterColumn.Stat.MOVE_SPEED_H] = data.MOVE_SPEED_H;
        targetDict[CharacterColumn.Stat.FIRE_RATE] = data.FIRE_RATE;
        targetDict[CharacterColumn.Stat.FIRE_RANGE] = data.FIRE_RANGE;
        targetDict[CharacterColumn.Stat.PENETRATE] = data.PENETRATE;
        targetDict[CharacterColumn.Stat.SPLASH_DAMAGE] = data.SPLASH_DAMAGE;
        targetDict[CharacterColumn.Stat.SPLASH_RANGE] = data.SPLASH_RANGE;
        targetDict[CharacterColumn.Stat.CRITICAL] = data.CRITICAL;
        targetDict[CharacterColumn.Stat.CRITICAL_DAMAGE] = data.CRITICAL_DAMAGE;
        targetDict[CharacterColumn.Stat.HP_DRAIN] = data.HP_DRAIN;
        targetDict[CharacterColumn.Stat.PROJECTILE_SPEED] = data.PROJECTILE_SPEED;
        targetDict[CharacterColumn.Stat.PROJECTILE_AMOUNT] = data.PROJECTILE_AMOUNT;
    }

    private void InitializeStats(CharacterData data, Dictionary<CharacterColumn.Stat, float> initialDict, Dictionary<CharacterColumn.Stat, float> targetDict)
    {
        InitializeStats(data, initialDict);
        foreach (var stat in initialDict)
        {
            targetDict[stat.Key] = stat.Value;
        }
    }

    private IEnumerator StopEffectAfter(WaitForSeconds sec)
    {
        yield return sec;
        foreach (var particle in effects)
        {
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        getOptionEffect.SetActive(false);
    }

    private void ConstraintCharacterAbility(OptionColumn.Stat stat)
    {
        if (stat == OptionColumn.Stat.ARMOR)
        {
            stats.stat[(CharacterColumn.Stat)stat] = Mathf.Min(stats.stat[(CharacterColumn.Stat)stat], 1.9f);
        }

        if (stats.stat[(CharacterColumn.Stat)stat] < 0)
        {
            stats.stat[(CharacterColumn.Stat)stat] = 0;
        }
    }

    private void ConstraintWeaponAbility(OptionColumn.Stat stat)
    {
        if (playerShooter.weapon.stats[(WeaponColumn.Stat)stat] < 0)
        {
            playerShooter.weapon.stats[(WeaponColumn.Stat)stat] = 0;
        }
    }
}