// # Systems
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

[System.Serializable]
public class CircleData
{
    public ColorType color;
    public GameObject prefab;
}

public class CirclePoolManager : MonoBehaviour
{
    public static CirclePoolManager Instance;

    [SerializeField] private List<CircleData> circles;

    private Dictionary<ColorType, GameObject> prefabDict;
    private Dictionary<ColorType, Stack<GameObject>> poolDict;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Initialize();
    }

    private void Initialize()
    {
        int dictLength = circles.Count;

        prefabDict = new Dictionary<ColorType, GameObject>(dictLength);
        poolDict = new Dictionary<ColorType, Stack<GameObject>>(dictLength);

        foreach(var circle in circles)
        {
            Register(circle);
        }
    }

    private void Register(CircleData circleData)
    {
        GameObject prefab = Instantiate(circleData.prefab);
        prefab.SetActive(false);

        Stack<GameObject> stack = new Stack<GameObject>();
        for (int i = 0; i < 10; i++)
        {
            GameObject go = Instantiate(circleData.prefab);
            stack.Push(go);
        }

        prefabDict.Add(circleData.color, prefab);
        poolDict.Add(circleData.color, stack);
    }

    private GameObject Clone(ColorType color)
    {
        GameObject go = Instantiate(prefabDict[color]);
        go.SetActive(false);
        poolDict[color].Push(go);

        return go;
    }

    public GameObject Spawn(ColorType color)
    {
        if (poolDict[color].Contains(prefabDict[color]))
        {
            return Clone(color);
        }

        GameObject go = poolDict[color].Pop();
        go.SetActive(true);

        return go;
    }

    public void DeSpawn(ColorType color, GameObject go)
    {
        if (!poolDict[color].Contains(go))
        {
            go.SetActive(false);
            poolDict[color].Push(go);
        }
    }
}