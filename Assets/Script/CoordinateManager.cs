using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
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

    const float evenX = -1.97f;
    const float oddX = -2.3f;

    const float evenY = 3.66f;
    const float oddY = 4.22f;

    const float plusX = 0.65f;
    const float plusY = 1.14f;

    private void Awake()
    {
        Instance = this;
        Initialization();
    }

    private void Initialization()
    {
        coordinates = new CoordinateData[105];

        InitCoordinate();
        InitCoordinatePosition();
    }

    private void InitCoordinate()
    {
        int y = 14;
        int index = 0;
        for (int i = 0; i < y; i++)
        {
            bool isOdd = (i + 1) % 2 != 0;
            int allocCnt = isOdd ? 8 : 7;

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
            bool isOdd = (i + 1) % 2 != 0;
            int allocCnt = isOdd ? 8 : 7;

            nowY = (isOdd ? oddY : evenY) - (i / 2) * plusY;

            for (int j = 0; j < allocCnt; j++)
            {
                 nowX = (isOdd ? oddX : evenX) + (j * plusX);
                 coordinates[index].coordinatePosition = new Vector2(nowX, nowY);
                 index++;
            }
        }
    }

    public void CheckCloseCoordinate(Circle centerCircle)
    {
        Stack<Circle> coordinateStack = new Stack<Circle>();
        coordinateStack.Push(centerCircle);

        while (coordinateStack.Count > 0)
        {
            Circle circle = coordinateStack.Pop();
            int CoordX = circle.myCoordinate.x;
            int CoordY = circle.myCoordinate.y;

            bool isOdd = (CoordY + 1) % 2 != 0;
            Vector2Int[] closeCircleVec = new Vector2Int[6];

            Debug.Log(isOdd);

            if (!isOdd)
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
                if (IsValid(closeCircleCoordinate, out Circle sameColorCircle, circle.colorType))
                {
                    foundedCircles.Push(sameColorCircle);
                    coordinateStack.Push(sameColorCircle);

                    Debug.Log(sameColorCircle.myCoordinate);
                }
            }
        }

        if (foundedCircles.Count > 2)
        {
            ExecuteBoomOnCircles();
        }
        else
        {
            foundedCircles.Clear();
        }
    }

    private bool IsValid(Vector2Int coordinate, out Circle sameColorCircle, ColorType colorType)
    {
        if(circleSaveDict.ContainsKey(coordinate))
        {
            sameColorCircle = circleSaveDict[coordinate];
            if (!foundedCircles.Contains(sameColorCircle) && sameColorCircle.colorType == colorType)
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

    public Vector2 GetCloseCoordinatePos(Vector2 circleVec, Circle circle)
    {
        FindCloseCoordinate(circleVec, out CoordinateData closeCoordinate);
        if(!circleSaveDict.ContainsKey(closeCoordinate.coordinate))
        {
            circleSaveDict.Add(closeCoordinate.coordinate, circle);
        }
        else
        {
            circleSaveDict[closeCoordinate.coordinate] = circle;
        }
       
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
                Debug.Log(coordinates[i].coordinatePosition);
                return coordinates[i].coordinatePosition;
            }
        }

        return Vector2.zero;
    }
}