using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public delegate void EndLevelAction();
    public static event EndLevelAction OnLevelEnd;

    private GameObject currentGround;
    private GameObject nextGround;

    [Header("Ground")]
    [SerializeField] private GameObject startGround;
    [SerializeField] private GameObject groundPrefab;
    [SerializeField] private Material[] groundMaterials;
    [SerializeField] private Vector3 groundOffset = new Vector3(10f, 0f, 0f);

    private void Awake()
    {
        currentGround = startGround;
        CreateNextLevel();
    }

    private void OnEnable()
    {
        SlimeController.OnLevelStart += StartLevel;
        EnemySpawner.OnAllEnemiesDeath += EndLevel;
    }

    private void OnDisable()
    {
        SlimeController.OnLevelStart -= StartLevel;
        EnemySpawner.OnAllEnemiesDeath -= EndLevel;
    }

    private void CreateNextLevel()
    {
        nextGround = Instantiate(groundPrefab, currentGround.transform.position + groundOffset, Quaternion.identity, transform);
        nextGround.GetComponent<MeshRenderer>().material = groundMaterials[Random.Range(0, groundMaterials.Length)];
    }

    private void EndLevel()
    {
        if (OnLevelEnd != null) OnLevelEnd();
        CreateNextLevel();
    }

    private void StartLevel()
    {
        Destroy(currentGround);
        currentGround = nextGround;
    }
}
