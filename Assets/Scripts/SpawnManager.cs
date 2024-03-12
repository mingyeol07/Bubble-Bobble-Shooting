using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPos;
    private Transform[] spawnPoint;

    private void Awake()
    {
        for (int i = 0; i < spawnPos.Length; i++)
        {
            
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Spawn();
        }
    }

    private void Spawn()
    {
        GameObject enemy = GameFacadeManager.poolManager.Get(0);
        Debug.Log(GameFacadeManager.poolManager.Get(0));
        enemy.transform.position = spawnPos[0].position;
    }
}
