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
    private List<Circle> foundedCircle = new List<Circle>();

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
        Vector2Int[] closeVec =  { new Vector2Int(x, y - 1), new Vector2Int(x + 1, y - 1), new Vector2Int(x + 1, y)
        , new Vector2Int(x + 1, y + 1), new Vector2Int(x, y + 1), new Vector2Int(x - 1, y)};

        
        foundedCircle.Add(centerCircle);

        foreach (Vector2Int coordinate in closeVec)
        {
            if(circleSaveDict.ContainsKey(coordinate))
            {
                Circle otherCircle = circleSaveDict[coordinate];

                if (otherCircle.colorType == centerCircle.colorType)
                {
                    Debug.Log("Ãß°¡");
                    if(!foundedCircle.Contains(otherCircle))
                    {
                        CircleFound(otherCircle);
                    }
                }
            }
        }
    }

    private void CircleFound(Circle otherCircle)
    {
        int foundedCount = foundedCircle.Count;

        foundedCircle.Add(otherCircle);

        if (foundedCount == foundedCircle.Count && foundedCircle.Count > 2)
        {
            for(int i =0;  i  < foundedCount; i++)
            {
                Destroy(foundedCircle[i].gameObject);
            }
            return;
        }
    }

    public Vector2 GetCloseCoordinatePos(Vector2 circleVec, Circle circle)
    {
        FindCloseCoordinate(circleVec, out CoordinateData closeCoordinate);
        if (!circleSaveDict.ContainsKey(closeCoordinate.coordinate))
        {
            circleSaveDict.Add(closeCoordinate.coordinate, circle);
        }
        else
        {
            circleSaveDict[closeCoordinate.coordinate] = circle;
        }
        return closeCoordinate.coordinatePosition;
    }

    private void FindCloseCoordinate(Vector2 circleVec, out CoordinateData coordinate)
    {
        CoordinateData closeCoordinate = null;
        float shortestDistance = float.MaxValue;

        for (int i = 0; i < coordinates.Length; i++)
        {
            float dist = Vector2.Distance(circleVec, coordinates[i].coordinatePosition);
            if (dist < shortestDistance)
            {
                shortestDistance = dist;
                closeCoordinate = coordinates[i];
            }
        }

        coordinate = closeCoordinate;
    }
}