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
    const float oddX = -8.15f;
    const float evenX = -7.82f;
   
    const float oddY = 4.22f;
    const float evenY = 3.66f;

    const float plusX = 0.65f;
    const float plusY = 1.14f;

    private void Awake()
    {
        Instance = this;
        Initialization();
    }

    private void Initialization()
    {
        coordinates = new CoordinateData[357];

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
            int allocCnt = isEven ? 26 : 25;

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
            int allocCnt = isEven ? 26 : 25;

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
        }
        else
        {
            foundedCircles.Clear();
        }
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

    /// <summary>
    /// 혼자 남겨진 공 검사
    /// </summary>
    private void CheckOnFallCircle()
    {
        for(int i =0; i < 26; i++)
        {
            if (IsValid(coordinates[i].coordinate, out Circle highCircle))
            {
                CircleFallCheck(highCircle);
            }
        }

        Stack<Circle> stack = new Stack<Circle>();

        foreach(Circle fallCircle in circleSaveDict.Values)
        {
            if (!foundedCircles.Contains(fallCircle) && fallCircle.myCoordinate.y != 0)
            {
                stack.Push(fallCircle);
            }
        }

        foreach (Circle fallCircle in stack)
        {
            fallCircle.Fall();
            circleSaveDict.Remove(fallCircle.myCoordinate);
        }

        foundedCircles.Clear();
    }

    private void CircleFallCheck(Circle centerCircle)
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
                    foundedCircles.Push(sameColorCircle);
                    coordinateStack.Push(sameColorCircle);
                }
            }
        }
    }

    public Vector2 GetCloseCoordinatePos(Vector2 circleVec, Circle circle)
    {
        FindCloseCoordinate(circleVec, out CoordinateData closeCoordinate);
        circleSaveDict.Add(closeCoordinate.coordinate, circle);

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