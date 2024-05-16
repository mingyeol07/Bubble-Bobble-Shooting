using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coordinate : MonoBehaviour
{
    private Vector2 myCoordinate;

    public void SetCoordinate(Vector2Int coordinate)
    {
        myCoordinate = coordinate;
    }
}
