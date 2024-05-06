using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using UnityEngine;

public class Circle : MonoBehaviour
{
    [SerializeField] private Circle otherCircle;
    private int currentCoordinateNumber;

    private void PositionSet()
    {
        transform.position = Coordinates.Instance.GetCloseCoordinate(transform.position, otherCircle.currentCoordinateNumber);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Circle"))
        {
            otherCircle = collision.gameObject.GetComponent<Circle>();
            PositionSet();
        }
    }
}
