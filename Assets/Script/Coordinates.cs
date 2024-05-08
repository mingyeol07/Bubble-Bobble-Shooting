using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinates : MonoBehaviour
{
    public static Coordinates Instance;

    [SerializeField] private Transform[] coordinates;
    private bool onCoordinate;
    private float x;
    private float y;
    private float distance;

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// ���� ��ġ���� ����� ��ǥ�� ã�Ƴ��� �Լ�
    /// </summary>
    /// <param name="circleVec">���� ��ġ</param>
    /// <returns></returns>
    public Vector2 GetCloseCoordinate(Vector2 circleVec)
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
            }
        }

        return closestCoordinate;
    }


    public Vector2 GetNumberCoordinate(int number)
    {
        return coordinates[number].position;
    }
}
