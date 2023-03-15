using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SlimeController : CharacterBehavior
{
    public delegate void UpdateStatAction(CharStat stat, int level, float bonus, float value);
    public static event UpdateStatAction OnStatUpdate;
    public delegate void StartLevelAction();
    public static event StartLevelAction OnLevelStart;
    public delegate void UpdateGoldAction(int gold);
    public static event UpdateGoldAction OnGoldUpdate;

    private float regenSpeed;
    private int totalGold; //TODO: refactor

    [Header("Player Stats")]
    [SerializeField] private float baseRegen = 0.1f;
    [SerializeField] private float regenPerLevel = 0.01f;

    [Header("Projectiles")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform projectilesTransform;

    protected override void Awake()
    {
        base.Awake();

        canMove = true;
        regenSpeed = baseRegen;
    }

    private void Start()
    {
        if (OnStatUpdate != null)
        {
            OnStatUpdate(CharStat.damage, statsLevel[(int)CharStat.damage], damagePerLevel, totalDamage);
            OnStatUpdate(CharStat.ats, statsLevel[(int)CharStat.ats], atsPerLevel, attackSpeed);
            OnStatUpdate(CharStat.health, statsLevel[(int)CharStat.health], healthPerLevel, maxHealth);
            OnStatUpdate(CharStat.regen, statsLevel[(int)CharStat.regen], regenPerLevel, regenSpeed);
        }

        if (OnGoldUpdate != null) OnGoldUpdate(totalGold);
    }

    private void OnEnable()
    {
        EnemyController.OnAttack += TakeDamage;
        EnemyController.OnDeath += TakeReward;
        UpgradeButton.OnUpgrade += UpgradeStat;
        LevelManager.OnLevelEnd += GoToNextLevel;
    }

    private void OnDisable()
    {
        EnemyController.OnAttack -= TakeDamage;
        EnemyController.OnDeath -= TakeReward;
        UpgradeButton.OnUpgrade -= UpgradeStat;
        LevelManager.OnLevelEnd -= GoToNextLevel;
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (currentHealth < maxHealth)
        {
            RegenHealth();
        }
    }

    private void RegenHealth()
    {
        ChangeHealth(maxHealth * regenSpeed * Time.deltaTime);
    }

    protected override void Attack()
    {
        base.Attack();

        GameObject projectile = Instantiate(projectilePrefab, projectilesTransform);
        projectile.GetComponent<ProjectileController>().SetDamage(baseDamage + damagePerLevel * statsLevel[(int)CharStat.damage]);
    }

    protected override void Die()
    {
        base.Die();

        SceneManager.LoadScene(0); //TODO
    }

    private void TakeReward(GameObject enemy, int reward)
    {
        AddGold(reward);
    }

    private void AddGold(int gold)
    {
        totalGold += gold;

        if (OnGoldUpdate != null) OnGoldUpdate(totalGold);
    }

    private void GoToNextLevel()
    {
        canAttack = false;

        foreach (Transform projectile in projectilesTransform)
        {
            Destroy(projectile.gameObject);
        }
    }

    private void UpgradeStat(CharStat stat, int cost)
    {
        int level = 0;
        float value = 0f;
        float bonus = 0f;

        switch (stat)
        {
            case CharStat.damage:
                level = ++statsLevel[(int)CharStat.damage];
                totalDamage += damagePerLevel;
                value = totalDamage;
                bonus = damagePerLevel;
                break;
            case CharStat.ats:
                level = ++statsLevel[(int)CharStat.ats];
                attackSpeed += atsPerLevel;
                value = attackSpeed;
                bonus = atsPerLevel;
                break;
            case CharStat.health:
                level = ++statsLevel[(int)CharStat.health];
                maxHealth += healthPerLevel;
                value = maxHealth;
                bonus = healthPerLevel;
                break;
            case CharStat.regen:
                level = ++statsLevel[(int)CharStat.regen];
                regenSpeed += regenPerLevel;
                value = regenSpeed;
                bonus = regenPerLevel;
                break;
        }

        if (OnStatUpdate != null) OnStatUpdate(stat, level, bonus, value);

        AddGold(-cost);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("LevelStart"))
        {
            Destroy(collider);
            canAttack = true;

            if (OnLevelStart != null) OnLevelStart();
        }
    }
}
