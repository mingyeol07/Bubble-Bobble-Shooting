using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinates : MonoBehaviour
{
    public static Coordinates Instance;

    [SerializeField] private Transform[] coordinates;
    private float x;
    private float y;
    private float distance;

    private void Awake()
    {
        Instance = this;
    }

    public Vector2 GetCloseCoordinate(Vector2 circleVec, int closeCoordinateNumber)
    {
        for (int i = closeCoordinateNumber - 1; i < coordinates.Length  i++)
        {
            x = circleVec.x - coordinates[i].transform.position.x;
            y = circleVec.y - coordinates[i].transform.position.y;
            distance = Mathf.Sqrt(x * x + y * y);

            if (distance < 0.75f)
            {
                return coordinates[i].position;
            }
        }

        return Vector2.zero;
    }

    public Vector2 GetNumberCoordinate(int number)
    {

    }
}
