using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using System;
using System.Collections;

public class PlayerWeaponData
{
    public WeaponColumn.Stat playerStat;
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
    public int price = default;

    private readonly string scoreFormat = "Score : {0}";
    public int exp;
    [Tooltip("최대 레벨")]
    public int maxLevel = 10;
    [Tooltip("레벨업에 필요한 경험치(즉, 스코어)")]
    public float expForNextLevel = 100;
    [Tooltip("경험치 배율")]
    public float magnification = 1.8f;


    private CharacterTable characterTable = null;
    public Dictionary<CharacterColumn.Stat, float> initialStats = new Dictionary<CharacterColumn.Stat, float>(); // 능력치 종류, 능력치 배율
    public Dictionary<CharacterColumn.Stat, float> stats = new Dictionary<CharacterColumn.Stat, float>(); // 능력치 종류, 능력치 배율
    public List<ItemData> items = new List<ItemData>();
    public List<PlayerWeaponData> playerWeaponDatas = new List<PlayerWeaponData>();
    [NonSerialized]
    public PrefabSelector prefabSelector = null;
    public CharacterData characterData = null;
    public string id = string.Empty;
    public float prevPositionZ = 1.5f;


    private void Awake()
    {
        characterTable = DataTableMgr.Get<CharacterTable>(DataTableIds.Character);

        if (!TryGetComponent(out prefabSelector))
        {
            prefabSelector.enabled = false;
            return;
        }

        effects = getOptionEffect.GetComponentsInChildren<ParticleSystem>();
        prevPositionZ = gameObject.transform.position.z;
    }

    private void OnEnable()
    {
        if (characterData == null)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((int)TableIdentifier.Character);
            sb.Append((prefabSelector.SelectedCharacterIndex + 1).ToString("D2"));
            sb.Append((prefabSelector.SelectedCharacterIndex + 1).ToString("D2")); // id + body(D2) + head(D2)
            id = sb.ToString();

            InitCharacterInfo(id);
        }
    }

    private void LateUpdate()
    {
        if ((int)(transform.position.z) > (int)prevPositionZ)
        {
            GetExp(1);
            prevPositionZ = transform.position.z;
        }
    }

    public bool UpdateStats(OptionColumn.Stat stat, OptionColumn.Type type, float value)
    {
        getOptionEffect.SetActive(true);
        foreach (var particle in effects)
        {
            particle.Play();
        }

        // 옵저버 패턴으로 stat을 observe하고 동작하는 클래스를 생성하여 변경 필요
        // Maximum이 있을 경우를 고려해 bool을 반환
        switch (type)
        {
            case OptionColumn.Type.Scale:
                {
                    stats[(CharacterColumn.Stat)stat] += initialStats[(CharacterColumn.Stat)stat] * (value / 100.0f);

                    if (stat == OptionColumn.Stat.ARMOR)
                    {
                        stats[(CharacterColumn.Stat)stat] = Mathf.Min(stats[(CharacterColumn.Stat)stat], 1.9f);
                    }

                    // 0 미만으로 떨어질 경우 0으로 반환
                    if(stats[(CharacterColumn.Stat)stat] < 0)
                    {
                        stats[(CharacterColumn.Stat)stat] = 0;
                    }
                }

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

                        playerWeaponDatas.Add(new PlayerWeaponData{ playerStat = (WeaponColumn.Stat)stat, playerValue = value });

                        if(playerShooter.weapon.stats[(WeaponColumn.Stat)stat] < 0)
                        {
                            playerShooter.weapon.stats[(WeaponColumn.Stat)stat] = 0;
                        }
                    }
                }
                break;
        }

        if (stat == OptionColumn.Stat.HP)
        {
            playerHealth.UpdateHealthUI();
        }

        if (stopEffectCoroutine != null)
        {
            StopCoroutine(stopEffectCoroutine);
        }

        stopEffectCoroutine = StartCoroutine(StopEffectAfter(twoSec));


        return true;
    }

    public void GetExp(int score) // 스코어를 exp로 사용
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
        // level up 효과 생성
        expForNextLevel *= magnification;
        var platformIndex = GameManager.Instance.currentPlatformIndex;
        var platform = GameManager.Instance.platforms[platformIndex].GetComponent<Platform>();
        platform.optionController.ResetOptions(level);
    }

    public void InitCharacterInfo(string id)
    {
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

        Debug.Log($"character projectile amount : {stats[CharacterColumn.Stat.PROJECTILE_AMOUNT]}");

        price = characterData.PRICE;
    }

    IEnumerator StopEffectAfter(WaitForSeconds sec)
    {
        yield return sec;
        foreach (var particle in effects)
        {
            particle.Stop();
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        getOptionEffect.SetActive(false);
    }
}
