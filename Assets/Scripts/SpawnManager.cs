using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private GameObject[] enemyPrefabs;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        GameObject Enemy = Instantiate(enemyPrefabs[0], transform);
        Enemy.transform.position = spawnPoints[3].position;
    }
}
