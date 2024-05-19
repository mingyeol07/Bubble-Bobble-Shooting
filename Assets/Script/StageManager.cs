// # Systems
using System.Collections;
using System.Collections.Generic;

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
    private List<Dictionary<string, object>> stage1Data;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        stage1Data = CSVReader.Read("Stage1");

        for (int i = 0; i < stage1Data.Count; i ++)
        {
            SetCircle((int)stage1Data[i]["X"], (int)stage1Data[i]["Y"], (int)stage1Data[i]["Color"]);
        }
    }

    private void SetCircle(int x, int y, int color)
    {
        GameObject go = Instantiate(circles[color], CoordinateManager.Instance.GetPositionToCoordinate(new Vector2Int(x, y)), Quaternion.identity);
        go.GetComponent<Circle>().SetPosition();
        go.GetComponent<CircleCollider2D>().enabled = true;
    }
}
