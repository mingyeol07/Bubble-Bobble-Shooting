using System;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

public class Coordinates : MonoBehaviour
{
    public static Coordinates Instance;

    [SerializeField] private Transform[] coordinates;
    private Dictionary<int, Circle> circleMap = new Dictionary<int, Circle>();
    int[] neighborOffsets = { -7, -8, -1, 1, 7, 8 };

    private void Awake()
    {
        Instance = this;
    }

    public Vector2 GetCloseCoordinate(Vector2 circleVec, Circle circle)
    {
        int closestIndex = FindClosestIndex(circleVec);
        circleMap[closestIndex] = circle;
        circle.index = closestIndex;
        CheckForSameColorCircles(closestIndex, circle.colorType);
        return coordinates[closestIndex].position;
    }

    public void CheckForSameColorCircles(int currentIndex, ColorType circleColor)
    {
        foreach (int offset in neighborOffsets)
        {
            int neighborIndex = currentIndex + offset;
            if (IsValidIndex(neighborIndex) && circleMap.ContainsKey(neighborIndex))
            {
                if (circleMap[neighborIndex] == null) continue;

                Circle neighborCircle = circleMap[neighborIndex];
                if (neighborCircle.colorType == circleColor)
                {
                    if (circleMap[neighborIndex] != null)
                    {
                        Debug.Log("dd");
                        ReMoveList(currentIndex);
                    }
                }
            }
        }
    }

    private int FindClosestIndex(Vector2 circleVec)
    {
        int closestIndex = -1;
        float shortestDistance = float.MaxValue;

        for (int i = 0; i < coordinates.Length; i++)
        {
            float dist = Vector2.Distance(circleVec, coordinates[i].position);
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
        return index >= 0 && index < coordinates.Length;
    }

    public void ReMoveList(int index)
    {
        if (circleMap.ContainsKey(index))
        {
            Destroy(circleMap[index]);
            circleMap.Remove(index);
        }
    }
}