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

    private Dictionary<ColorType, Stack<GameObject>> poolDict;

    private void Awake()
    {
        Instance = this;
        Initialize();
    }

    private void Initialize()
    {
        poolDict = new Dictionary<ColorType, Stack<GameObject>>();

        foreach (var circleData in circles)
        {
            GameObject prefab = circleData.prefab;
            Stack<GameObject> stack = new Stack<GameObject>();

            for (int i = 0; i < 10; i++)
            {
                GameObject go = Instantiate(prefab);
                go.SetActive(false);
                stack.Push(go);
            }

            poolDict.Add(circleData.color, stack);
        }
    }

    public GameObject Spawn(ColorType color)
    {
        if (!poolDict.ContainsKey(color))
        {
            Debug.LogWarning("No prefab found for color: " + color);
            return null;
        }
       
        GameObject go;
        if (poolDict[color].Count == 0)
        {
            go = Instantiate(circles.Find(circle => circle.color == color).prefab);
        }
        else
        {
            go = poolDict[color].Pop();
        }

        go.SetActive(true);
        return go;
    }

    public void DeSpawn(ColorType color, GameObject go)
    {
        if (!poolDict.ContainsKey(color))
        {
            Debug.LogWarning("No prefab found for color: " + color);
            return;
        }

        go.SetActive(false);
        poolDict[color].Push(go);
    }
}