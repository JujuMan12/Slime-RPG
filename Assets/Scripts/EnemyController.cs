using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : CharacterBehavior
{
    public delegate void AttackAction(int damage);
    public static event AttackAction OnAttack;
    public delegate void DeathAction(GameObject enemy, int reward);
    public static event DeathAction OnDeath;

    [Header("Enenmy Stats")]
    [SerializeField] private int reward = 50;

    private void OnEnable()
    {
        SlimeController.OnLevelStart += StartMoving;
    }

    private void OnDisable()
    {
        SlimeController.OnLevelStart -= StartMoving;
    }

    private void StartMoving()
    {
        canMove = true;
    }

    protected override void Attack()
    {
        base.Attack();

        if (OnAttack != null) OnAttack(totalDamage);
    }

    protected override void Die()
    {
        if (OnDeath != null) OnDeath(gameObject, reward);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Player"))
        {
            canAttack = true;
        }
    }
}
