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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        RandomCircle();
        NextCircleInit();
    }

    private void RandomCircle()
    {
        HashSet<int> indexes = new HashSet<int>(); // 중복되지 않는 데이터들을 저장하는 HashSet

        while (indexes.Count < prefab.Count)
        {
            int randomIndex = UnityEngine.Random.Range(0, prefab.Count);
            if (!indexes.Contains(randomIndex))
            {
                circleQueue.Enqueue(prefab[randomIndex]);
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