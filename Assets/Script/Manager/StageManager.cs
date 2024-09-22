// # Systems
using System.Collections;
using System.Collections.Generic;
using System.Drawing;


// # Unity
using UnityEngine;

// 임시 스테이지 클래스
public class StageManager : MonoBehaviour
{
    public static StageManager Instance;

    /// <summary>
    /// circle의 순서는 ColorType의 color순서대로 맞춰야한다.
    /// </summary>
    [SerializeField] private List<GameObject> circles = new List<GameObject>();
    private List<Dictionary<string, object>> stageData;
    private CoordinateManager coordinateManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        coordinateManager = CoordinateManager.Instance;
        RandomSpawn();
    }

    private void SetStagedata(int stageNumber)
    {
        stageData = CSVReader.Read("Stage" + stageNumber.ToString());

        for (int i = 0; i < stageData.Count; i++)
        {
            SetCircle((int)stageData[i]["X"], (int)stageData[i]["Y"], (int)stageData[i]["Color"]);
        }
    }

    private void SetCircle(int x, int y, int color)
    {
        GameObject go = Instantiate(circles[color], coordinateManager.GetPositionToCoordinate(new Vector2Int(x, y)), Quaternion.identity);
        go.GetComponent<Circle>().SetPosition();
        go.GetComponent<CircleCollider2D>().enabled = true;
    }

    private void SetCirdle(CoordinateData coordinateData)
    {
        GameObject go = Instantiate(circles[Random.Range(0, circles.Count)], coordinateData.coordinatePosition, Quaternion.identity);
        go.GetComponent<Circle>().SetPosition();
        go.GetComponent<CircleCollider2D>().enabled = true;
    }

    private void RandomSpawn()
    {
        CoordinateData[] coordinates = coordinateManager.coordinates;

        for(int i =0; i < coordinates.Length - 51 - 25 - 26; i ++)
        {
            SetCirdle(coordinates[i]);
        }
}
}
