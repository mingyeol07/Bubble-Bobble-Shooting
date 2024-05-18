using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class CoordinateManager : MonoBehaviour
{
    public static CoordinateManager Instance;

    [SerializeField] private CoordinateData[] coordinates;
    private Dictionary<Vector2Int, Circle> circleSaveDict = new Dictionary<Vector2Int, Circle>();
    private HashSet<Circle> foundedCircle = new HashSet<Circle>();

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

    public void CheckCloseCoordinate(int x, int y, Circle centerCircle)
    {
        Vector2Int[] closeVec =  { new Vector2Int(x, y - 1), new Vector2Int(x + 1, y - 1), new Vector2Int(x + 1, y),
                               new Vector2Int(x + 1, y + 1), new Vector2Int(x, y + 1), new Vector2Int(x - 1, y)};

        if (!foundedCircle.Contains(centerCircle))
            foundedCircle.Add(centerCircle);

        int initialCount = foundedCircle.Count;

        foreach (Vector2Int coordinate in closeVec)
        {
            if (IsVaild(coordinate))
            {

            }
            else continue;

            if (circleSaveDict.ContainsKey(coordinate))
            {
                Circle sameColorCircle = circleSaveDict[coordinate];

                if (sameColorCircle.colorType == centerCircle.colorType)
                {
                    if (!foundedCircle.Contains(sameColorCircle))
                    {
                        foundedCircle.Add(sameColorCircle);
                        sameColorCircle.CheckColor();
                    }

                    if (foundedCircle.Count == initialCount && foundedCircle.Count > 2)
                    {
                        foreach (Circle circle in foundedCircle)
                        {
                            Destroy(circle.gameObject);
                            circleSaveDict.Remove(circle.myCoordinate);
                        }
                    }
                }
            }
        }

        foundedCircle.Clear();
    }

    private bool IsVaild(Vector2Int coordinate)
    {
        if(circleSaveDict.ContainsKey(coordinate))
        {
            return true;
        }
        return false;
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
}