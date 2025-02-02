// # Systems
using System;
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class ReloadManager : MonoBehaviour
{
    public static ReloadManager Instance;

    private GameObject nextCircle;
    private GameObject prevCircle;
    [SerializeField] private Transform player;
    [SerializeField] private Transform startPos;
    [SerializeField] private Transform reloadPos;

    public bool reloadExit = false;

    [SerializeField] private List<GameObject> prefab = new List<GameObject>();
    private Queue<GameObject> circleQueue = new Queue<GameObject>();
    private GameObject[] useGameObjects;

    [SerializeField] private Shooter shooter;

    private void Awake()
    {
        Instance = this;
    }

    public void StartReloading(ColorType[] colorTypes)
    {
        useGameObjects = new GameObject[colorTypes.Length];
        for(int i = 0; i < colorTypes.Length; i++)
        {
            useGameObjects[i] = prefab[(int)colorTypes[i]];
        }
        Destroy(prevCircle);
        prevCircle = null;
        shooter.DestroyCircle();
        RandomCircle();
        NextCircleInit();
        StartCoroutine(shooter.StartSetCircle());
    }

    private void RandomCircle()
    {
        HashSet<int> indexes = new HashSet<int>(); // 중복되지 않는 데이터들을 저장하는 HashSet

        while (indexes.Count < useGameObjects.Length)
        {
            int randomIndex = UnityEngine.Random.Range(0, useGameObjects.Length);
            if (!indexes.Contains(randomIndex))
            {
                circleQueue.Enqueue(useGameObjects[randomIndex]);
                indexes.Add(randomIndex);
            }
        }
    }

    private void NextCircleInit()
    {
        if(nextCircle != null) prevCircle = nextCircle;
        if (circleQueue.Count <= 1) RandomCircle();
        nextCircle = Instantiate(circleQueue.Dequeue());
        nextCircle.GetComponent<Animator>().SetTrigger("Start");
        nextCircle.transform.parent = player;
    }

    public GameObject GetShootCircle()
    {
        NextCircleInit();
        GameObject circle = prevCircle;
        reloadExit = false;
        circle.GetComponent<Animator>().SetTrigger("Reload");
        circle.transform.position = reloadPos.position;
        return circle;
    }

    public void ReloadExit()
    {
        reloadExit = true;
    }
}