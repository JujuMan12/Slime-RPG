using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterBehavior : MonoBehaviour
{
    public enum CharStat { damage, ats, health, regen }
    protected int[] statsLevel;
    protected bool canAttack;
    protected bool canMove;
    private float timeToAttack;
    protected int totalDamage;
    protected float attackSpeed;
    protected float currentHealth;
    protected int maxHealth;

    [Header("Common Stats")]
    [SerializeField] private float movementSpeed = 5f;
    [SerializeField] protected int baseDamage = 10;
    [SerializeField] protected int damagePerLevel = 1;
    [SerializeField] protected int baseHealth = 100;
    [SerializeField] protected int healthPerLevel = 10;
    [SerializeField] private float baseAts = 0.5f;
    [SerializeField] protected float atsPerLevel = 0.05f;
    [SerializeField] private Transform healthBar;
    [SerializeField] private GameObject damageDonePrefab;

    protected virtual void Awake()
    {
        statsLevel = new int[4];

        totalDamage = baseDamage;
        attackSpeed = baseAts;
        currentHealth = baseHealth;
        maxHealth = baseHealth;
    }

    protected virtual void FixedUpdate()
    {
        if (canAttack)
        {
            CheckAttack();
        }
        else if (canMove)
        {
            MoveForward();
        }
    }

    private void CheckAttack()
    {
        if (timeToAttack <= 0f)
        {
            Attack();
        }
        else
        {
            timeToAttack -= Time.deltaTime;
        }
    }

    protected virtual void Attack()
    {
        timeToAttack = 1f / (baseAts + atsPerLevel * statsLevel[(int)CharStat.ats]);
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
    }

    public void TakeDamage(int damage) //TODO: protected
    {
        ChangeHealth(-damage);

        GameObject damageDone = Instantiate(damageDonePrefab, transform);
        damageDone.GetComponent<DamageEffect>().SetDamage(damage);

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected void ChangeHealth(float healthChange)
    {
        currentHealth += healthChange;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

        healthBar.localScale = new Vector3(currentHealth / maxHealth, 1f, 1f);
    }
}
