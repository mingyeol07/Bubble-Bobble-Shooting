// # Systems
using System.Collections;
using System.Collections.Generic;
using System.Linq;




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
    [SerializeField] private GoogleSheetLoader sheetLoader = new GoogleSheetLoader();

    private int stageNumber = 0;
    private CoordinateManager coordinateManager;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        coordinateManager = CoordinateManager.Instance;
        //RandomSpawn();

    }

    public void StartStage(int stageNumber)
    {
        StartCoroutine(SetStagedata(stageNumber));
    }
    public void ClearStage()
    {
        stageNumber++;
        StartStage(stageNumber);
    }

    private IEnumerator SetStagedata(int stageNumber)
    {
        yield return StartCoroutine(sheetLoader.SetListCircleInSheet(stageNumber));
        CircleData[] stageData = sheetLoader.CircleDatas;
        HashSet<ColorType> circleColors = new HashSet<ColorType>();

        for (int i = 0; i < stageData.Length; i++)
        {
            SetCircle(stageData[i].x, stageData[i].y, (int)stageData[i].color);
            circleColors.Add(stageData[i].color);
        }

        yield return null;
        ReloadManager.Instance.StartReloading(circleColors.ToArray());
    }

    private void SetCircle(int x, int y, int color)
    {
        GameObject go = Instantiate(circles[color], coordinateManager.GetPositionToCoordinate(new Vector2Int(x, y)), Quaternion.identity);
        go.GetComponent<Circle>()?.SetPosition();
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
