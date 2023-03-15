using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private EnemyController target;
    private int damage;

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 5f;

    private void FixedUpdate()
    {
        if (target != null)
        {
            MoveToTarget();
        }
        else
        {
            MoveForward();
        }
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, movementSpeed * Time.deltaTime);
    }

    private void MoveForward()
    {
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
    }

    public void SetDamage(int totalDamage)
    {
        damage = totalDamage;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Enemy") && target == null)
        {
            target = collider.GetComponent<EnemyController>();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();

            enemy.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
