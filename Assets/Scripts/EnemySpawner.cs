using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public delegate void EnemiesDeathAction();
    public static event EnemiesDeathAction OnAllEnemiesDeath;

    private List<GameObject> enemies;

    [Header("Enemies")]
    [SerializeField] private int enemiesToSpawn;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private List<Transform> spawnPoints;

    private void Awake()
    {
        SpawnEnemies();
    }

    private void OnEnable()
    {
        EnemyController.OnDeath += RemoveEnemy;
    }

    private void OnDisable()
    {
        EnemyController.OnDeath -= RemoveEnemy;
    }

    private void SpawnEnemies()
    {
        enemies = new List<GameObject>();

        while (enemiesToSpawn > 0)
        {
            int randomPoint = Random.Range(0, spawnPoints.Count);
            enemies.Add(Instantiate(enemyPrefab, spawnPoints[randomPoint]));

            spawnPoints.RemoveAt(randomPoint);
            enemiesToSpawn--;
        }
    }

    private void RemoveEnemy(GameObject enemy, int reward)
    {
        enemies.Remove(enemy);
        Destroy(enemy);

        if (enemies.Count == 0 && OnAllEnemiesDeath != null) OnAllEnemiesDeath();
    }
}
