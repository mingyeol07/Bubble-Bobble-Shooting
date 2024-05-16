using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class CoordinateManager : MonoBehaviour
{
    public static CoordinateManager Instance;

    private Dictionary<int, Circle> circleMap = new Dictionary<int, Circle>();
    private int[] neighborOffsets = { -7, -8, -1, 1, 7, 8 };
    private HashSet<int> foundCircle = new HashSet<int>();

    [SerializeField] private Vector2Int[] coordinates;
    [SerializeField] private Vector2[] coordinatePos ;
    const float evenX = -1.97f;
    const float oddX = -2.3f;

    const float evenY = 3.66f;
    const float oddY = 4.22f;

    const float plusX = 0.65f;
    const float plusY = 1.14f;

    private void Awake()
    {
        Instance = this;
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
                Vector2Int coordinateIndex = new Vector2Int(j, i);
                coordinates[index] = coordinateIndex;
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
                 coordinatePos[index] = new Vector2(nowX, nowY);
                 index++;
            }
        }
    }

    private void CheckCloseCoordinate(int x, int y)
    {
        Vector2Int[] closeVec =  { new Vector2Int(x, y - 1), new Vector2Int(x + 1, y - 1), new Vector2Int(x + 1, y)
        , new Vector2Int(x + 1, y + 1), new Vector2Int(x, y + 1), new Vector2Int(x - 1, y)};

        for (int i = 0; i < closeVec.Length; i++)
        {
            foreach(Vector2Int coordinate in coordinates)
            {
                if (closeVec[i].Equals(coordinate))
                {

                }
            }
        }
    }

    public Vector2 GetCloseCoordinate(Vector2 circleVec, Circle circle)
    {
        int closestIndex = FindClosestIndex(circleVec);
        circleMap[closestIndex] = circle;
        circle.index = closestIndex;
        return coordinatePos[closestIndex];
    }

    public void CheckForSameColorCircles(int currentIndex, ColorType circleColor)
    {
        int beforeFoundCircles = foundCircle.Count;
        
        foreach (int offset in neighborOffsets)
        {
            int neighborIndex = currentIndex + offset;
            if (IsValidIndex(neighborIndex) && circleMap.ContainsKey(neighborIndex))
            {
                if (circleMap[neighborIndex] == null) continue;

                Circle neighborCircle = circleMap[neighborIndex];
                if (neighborCircle.colorType == circleColor)
                {
                    if (!foundCircle.Contains(neighborIndex)) // 이미 찾은 원이 아니라면
                    {
                        foundCircle.Add(neighborIndex); // foundCircle에 추가
                        neighborCircle.CheckColor();
                    }
                }
            }
        }

        if (foundCircle.Count == beforeFoundCircles && foundCircle.Count > 0)
        {
            foundCircle.Add(currentIndex);
            DestroyCircles();
        }
    }

    public void DestroyCircles()
    {
        foreach (int index in foundCircle)
        {
            ReMoveList(index);
        }
        foundCircle.Clear(); // foundCircle 초기화
    }

    private int FindClosestIndex(Vector2 circleVec)
    {
        int closestIndex = -1;
        float shortestDistance = float.MaxValue;

        for (int i = 0; i < coordinatePos.Length; i++)
        {
            float dist = Vector2.Distance(circleVec, coordinatePos[i]);
            if (dist < shortestDistance)
            {
                shortestDistance = dist;
                closestIndex = i;
            }
        }
        return closestIndex;
    }

    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < coordinatePos.Length;
    }

    public void ReMoveList(int index)
    {
        if (circleMap.ContainsKey(index))
        {
            circleMap[index].Boom();
            circleMap.Remove(index);
        }
    }
}