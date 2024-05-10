using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinates : MonoBehaviour
{
    public static Coordinates Instance;

    [SerializeField] private Transform[] coordinates;
    private Circle[] circles;
    private int currentCircleNumber;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        circles = new Circle[coordinates.Length];
    }

    /// <summary>
    /// 나의 위치에서 가까운 좌표를 찾아내는 함수
    /// </summary>
    /// <param name="circleVec">나의 위치</param>
    /// <returns></returns>
    public Vector2 GetCloseCoordinate(Vector2 circleVec, Circle circle)
    {
        Vector2 closestCoordinate = Vector2.zero;
        float shortestDistance = float.MaxValue;

        for (int i = 0; i < coordinates.Length; i++)
        {
            float dist = Vector2.Distance(circleVec, coordinates[i].position);

            if (dist < shortestDistance)
            {
                shortestDistance = dist;
                closestCoordinate = coordinates[i].position;
                currentCircleNumber = i;
            }
        }

        circles[currentCircleNumber] = circle;
        DisableCheckSameColorCircles();

        return closestCoordinate;
    }

    public void DisableCheckSameColorCircles()
    {
        DisableCircle(-7);
        DisableCircle(-8);
        DisableCircle(-1);
        DisableCircle(+1);
        DisableCircle(+7);
        DisableCircle(+8);
    }

    private void DisableCircle(int plusNumber)
    {
        int checkNumber = currentCircleNumber + plusNumber;

        if(checkNumber >= 0)
        {
            if (circles[checkNumber] != null && circles[currentCircleNumber].color == circles[currentCircleNumber].color)
            {
                circles[currentCircleNumber].CloseSameColor();
                circles[checkNumber].CloseSameColor();
            }
        }
    }
}
