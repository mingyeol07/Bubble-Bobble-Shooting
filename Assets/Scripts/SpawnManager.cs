using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    [SerializeField] private Transform[] spawnPoints;
    [SerializeField] private Transform[] stopPoints;
    [SerializeField] private GameObject[] enemyPrefabs;
   
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(SpawnAction());
    }

    IEnumerator SpawnAction()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            StraightSpawn(0, i);
            yield return new WaitForSeconds(0.3f);
        }
    }

    private void StraightSpawn(int enemyNumber, int point)
    {
        GameObject Enemy = Instantiate(enemyPrefabs[enemyNumber], transform);
        Enemy.transform.position = spawnPoints[point].position;
        Enemy.GetComponent<EnemySetupPosition>().SpawnMove(stopPoints[point]);
    }
}
