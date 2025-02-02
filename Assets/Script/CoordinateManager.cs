using System;
using System.Collections.Generic;
using UnityEngine;

public class CoordinateManager : MonoBehaviour
{
    public static CoordinateManager Instance;

    public CoordinateData[] coordinates;
    private Dictionary<Vector2Int, Circle> circleSaveDict = new Dictionary<Vector2Int, Circle>();
    /// <summary>
    /// 누적된 Cricle들을 담는 변수
    /// </summary>
    private Stack<Circle> foundedCircles = new Stack<Circle>();
    // odd -2.3, even -1.97

    #region 변칙게임 초기화변수
    private float oddX = -8.15f;
    private float evenX = -7.82f;

    private float oddY = 4.22f;
    private float evenY = 3.66f;

    private float plusX = 0.65f;
    private float plusY = 1.14f;

    private int XSize = 25;

    private int arraySize = 357;
    #endregion

    [SerializeField] private bool isNormalGame;

    const float no_oddX = -2.3f;
    const float no_evenX = -1.97f;

    const float no_oddY = 4.22f;
    const float no_evenY = 3.66f;

    const float no_plusX = 0.65f;
    const float no_plusY = 1.14f;

    const int no_XSize = 7;

    const int no_arraySize = 105;

    private void Awake()
    {
        Instance = this;
        if (isNormalGame)
        {
            oddX = no_oddX;
            oddY = no_oddY;

            evenX = no_evenX;
            evenY = no_evenY;

            plusX = no_plusX;
            plusY = no_plusY;

            XSize = no_XSize;

            arraySize = no_arraySize;
        }

        Initialization();
    }

    private void Start()
    {
        StageManager.Instance.StartStage(0);
    }

    private void Initialization()
    {
        coordinates = new CoordinateData[arraySize];

        InitCoordinate();
        InitCoordinatePosition();
    }

    private void InitCoordinate()
    {
        int y = 14;
        int index = 0;
        for (int i = 0; i < y; i++)
        {
            bool isEven = (i + 1) % 2 != 0;
            int allocCnt = isEven ? XSize + 1 : XSize;

            for (int j = 0; j < allocCnt; j++)
            {
                coordinates[index] = new CoordinateData();
                Vector2Int coordinateIndex = new Vector2Int(j, i);
                coordinates[index].coordinate = coordinateIndex;
                index++;
            }
        }
    }

    private void InitCoordinatePosition()
    {
        int y = 14;
        int index = 0;

        float nowX;
        float nowY;

        for (int i =0; i < y; i++)
        {
            bool isEven = (i + 1) % 2 != 0;
            int allocCnt = isEven ? XSize + 1 : XSize;

            nowY = (isEven ? oddY : evenY) - (i / 2) * plusY;

            for (int j = 0; j < allocCnt; j++)
            {
                 nowX = (isEven ? oddX : evenX) + (j * plusX);
                 coordinates[index].coordinatePosition = new Vector2(nowX, nowY);
                 index++;
            }
        }
    }

    /// <summary>
    /// 중심 공 기준 주변 6개 칸에 똑같은 색의 공이 있는지 검사
    /// </summary>
    /// <param name="centerCircle">중심 공</param>
    public void CheckCloseCoordinate(Circle centerCircle)
    {
        Stack<Circle> coordinateStack = new Stack<Circle>();
        coordinateStack.Push(centerCircle);

        while (coordinateStack.Count > 0)
        {
            Circle circle = coordinateStack.Pop();
            int CoordX = circle.myCoordinate.x;
            int CoordY = circle.myCoordinate.y;

            bool isEven = (CoordY + 1) % 2 != 0;
            Vector2Int[] closeCircleVec = new Vector2Int[6];

            if (!isEven)
            {
                closeCircleVec = new Vector2Int[6] { new Vector2Int(CoordX, CoordY - 1), new Vector2Int(CoordX + 1, CoordY - 1), new Vector2Int(CoordX + 1, CoordY),
                               new Vector2Int(CoordX + 1, CoordY + 1), new Vector2Int(CoordX, CoordY + 1), new Vector2Int(CoordX - 1, CoordY)};
            }
           else
            {
                closeCircleVec = new Vector2Int[6]  { new Vector2Int(CoordX - 1, CoordY - 1), new Vector2Int(CoordX, CoordY - 1), new Vector2Int(CoordX + 1, CoordY),
                               new Vector2Int(CoordX, CoordY + 1), new Vector2Int(CoordX - 1, CoordY + 1), new Vector2Int(CoordX - 1, CoordY)};
            }

            foreach (Vector2Int closeCircleCoordinate in closeCircleVec)
            {
                if (IsValid(closeCircleCoordinate, out Circle sameColorCircle))
                {
                    if( sameColorCircle.colorType == circle.colorType)
                    {
                        foundedCircles.Push(sameColorCircle);
                        coordinateStack.Push(sameColorCircle);
                    }
                }
            }
        }

        if (foundedCircles.Count > 2)
        {
            ExecuteBoomOnCircles();
            CheckOnFallCircle();
            if(IsClearStage())
            {
                circleSaveDict.Clear();
                StageManager.Instance.ClearStage();
            }
        }
        else
        {
            foundedCircles.Clear();
        }
    }

    private bool IsClearStage()
    {
        for(int i = 0; i < 8; i++)
        {
            if(IsValid(coordinates[i].coordinate, out Circle highCircle))
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// 좌표에 오브젝트가 있는지, 그 오브젝트가 이미 저장된 것인지 검사
    /// </summary>
    /// <param name="coordinate"></param>
    /// <param name="sameColorCircle"></param>
    /// <returns></returns>
    private bool IsValid(Vector2Int coordinate, out Circle sameColorCircle)
    {
        if(circleSaveDict.ContainsKey(coordinate))
        {
            sameColorCircle = circleSaveDict[coordinate];
            if (!foundedCircles.Contains(sameColorCircle))
            {
                return true;
            }
        }

        sameColorCircle = null;
        return false;
    }

    private void ExecuteBoomOnCircles()
    {
        while (foundedCircles.Count > 0 )
        {
            Circle circle = foundedCircles.Pop();
            circle.Boom();
            circleSaveDict.Remove(circle.myCoordinate);
        }
        foundedCircles.Clear();
    }

    private void CheckOnFallCircle()
    {
        HashSet<Circle> connectedCircles = new HashSet<Circle>(); // 천장과 연결된 버블들
        Queue<Circle> queue = new Queue<Circle>();

        // 1. 천장에 있는 버블들 찾기
        foreach (Circle circle in circleSaveDict.Values)
        {
            if (circle.myCoordinate.y == 0) // y가 0이면 천장에 붙어 있음
            {
                queue.Enqueue(circle);
                connectedCircles.Add(circle);
            }
        }

        // 2. BFS로 천장과 연결된 모든 버블 찾기
        while (queue.Count > 0)
        {
            Circle current = queue.Dequeue();
            foreach (Circle neighbor in GetNeighbors(current)) // 인접 버블 가져오기
            {
                if (!connectedCircles.Contains(neighbor))
                {
                    connectedCircles.Add(neighbor);
                    queue.Enqueue(neighbor);
                }
            }
        }

        // 3. 연결되지 않은 버블 찾기
        List<Circle> fallingCircles = new List<Circle>();
        foreach (Circle circle in circleSaveDict.Values)
        {
            if (!connectedCircles.Contains(circle)) // 천장과 연결되지 않은 버블 찾기
            {
                fallingCircles.Add(circle);
            }
        }

        // 4. 연결되지 않은 버블 제거
        foreach (Circle fallingCircle in fallingCircles)
        {
            fallingCircle.Fall(); // 애니메이션 적용
            circleSaveDict.Remove(fallingCircle.myCoordinate);
        }
    }

    private List<Circle> GetNeighbors(Circle circle)
    {
        List<Circle> neighbors = new List<Circle>();
        int CoordX = circle.myCoordinate.x;
        int CoordY = circle.myCoordinate.y;

        // 짝수 행인지 홀수 행인지 판별
        bool isEven = (CoordY + 1) % 2 != 0;
        Vector2Int[] closeCircleVec;

        if (!isEven)
        {
            closeCircleVec = new Vector2Int[6]
            {
            new Vector2Int(CoordX, CoordY - 1),
            new Vector2Int(CoordX + 1, CoordY - 1),
            new Vector2Int(CoordX + 1, CoordY),
            new Vector2Int(CoordX + 1, CoordY + 1),
            new Vector2Int(CoordX, CoordY + 1),
            new Vector2Int(CoordX - 1, CoordY)
            };
        }
        else
        {
            closeCircleVec = new Vector2Int[6]
            {
            new Vector2Int(CoordX - 1, CoordY - 1),
            new Vector2Int(CoordX, CoordY - 1),
            new Vector2Int(CoordX + 1, CoordY),
            new Vector2Int(CoordX, CoordY + 1),
            new Vector2Int(CoordX - 1, CoordY + 1),
            new Vector2Int(CoordX - 1, CoordY)
            };
        }

        // 유효한 좌표에 있는 Circle만 neighbors 리스트에 추가
        foreach (Vector2Int neighborCoord in closeCircleVec)
        {
            if (circleSaveDict.TryGetValue(neighborCoord, out Circle neighbor))
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }

    /// <summary>
    /// 혼자 남겨진 공 검사
    /// </summary>
    //private void CheckOnFallCircle()
    //{
    //    for(int i =0; i < 26; i++)
    //    {
    //        if (IsValid(coordinates[i].coordinate, out Circle highCircle))
    //        {
    //            CircleFallCheck(highCircle);
    //        }
    //    }

    //    Stack<Circle> stack = new Stack<Circle>();

    //    foreach(Circle fallCircle in circleSaveDict.Values)
    //    {
    //        if (!foundedCircles.Contains(fallCircle) && fallCircle.myCoordinate.y != 0)
    //        {
    //            stack.Push(fallCircle);
    //        }
    //    }

    //    foreach (Circle fallCircle in stack)
    //    {
    //        fallCircle.Fall();
    //        circleSaveDict.Remove(fallCircle.myCoordinate);
    //    }

    //    foundedCircles.Clear();
    //}

    //private void CircleFallCheck(Circle centerCircle)
    //{
    //    Stack<Circle> coordinateStack = new Stack<Circle>();
    //    coordinateStack.Push(centerCircle);

    //    while (coordinateStack.Count > 0)
    //    {
    //        Circle circle = coordinateStack.Pop();
    //        int CoordX = circle.myCoordinate.x;
    //        int CoordY = circle.myCoordinate.y;

    //        bool isEven = (CoordY + 1) % 2 != 0;
    //        Vector2Int[] closeCircleVec = new Vector2Int[6];

    //        if (!isEven)
    //        {
    //            closeCircleVec = new Vector2Int[6] { new Vector2Int(CoordX, CoordY - 1), new Vector2Int(CoordX + 1, CoordY - 1), new Vector2Int(CoordX + 1, CoordY),
    //                           new Vector2Int(CoordX + 1, CoordY + 1), new Vector2Int(CoordX, CoordY + 1), new Vector2Int(CoordX - 1, CoordY)};
    //        }
    //        else
    //        {
    //            closeCircleVec = new Vector2Int[6]  { new Vector2Int(CoordX - 1, CoordY - 1), new Vector2Int(CoordX, CoordY - 1), new Vector2Int(CoordX + 1, CoordY),
    //                           new Vector2Int(CoordX, CoordY + 1), new Vector2Int(CoordX - 1, CoordY + 1), new Vector2Int(CoordX - 1, CoordY)};
    //        }

    //        foreach (Vector2Int closeCircleCoordinate in closeCircleVec)
    //        {
    //            if (IsValid(closeCircleCoordinate, out Circle sameColorCircle))
    //            {
    //                foundedCircles.Push(sameColorCircle);
    //                coordinateStack.Push(sameColorCircle);
    //            }
    //        }
    //    }
    //}

    public Vector2 GetCloseCoordinatePos(Vector2 circleVec, Circle circle)
    {
        FindCloseCoordinate(circleVec, out CoordinateData closeCoordinate);
        Debug.Log(circleVec);
        Debug.Log(closeCoordinate.coordinate);
        if(!circleSaveDict.ContainsKey(closeCoordinate.coordinate)) circleSaveDict.Add(closeCoordinate.coordinate, circle);

        circle.myCoordinate = closeCoordinate.coordinate;
        return closeCoordinate.coordinatePosition;
    }

    private void FindCloseCoordinate(Vector2 circleVec, out CoordinateData coordinate)
    {
        CoordinateData closeCoordinate = null;
        float shortestDistance = float.MaxValue;

        for (int i = 0; i < coordinates.Length; i++)
        {
            float distance = Vector2.Distance(circleVec, coordinates[i].coordinatePosition);
            if (distance < shortestDistance)
            {
                shortestDistance = distance;
                closeCoordinate = coordinates[i];
            }
        }

        coordinate = closeCoordinate;
    }

    public Vector3 GetPositionToCoordinate(Vector2Int coordinate)
    {
        for (int i = 0; i<coordinates.Length; i++)
        {
            if(coordinates[i].coordinate == coordinate)
            {
                return coordinates[i].coordinatePosition;
            }
        }

        return Vector2.zero;
    }
}