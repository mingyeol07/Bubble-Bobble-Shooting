using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private GameObject bullet;
    [Header("Aim")]
    [SerializeField] private Transform shootPosition;

    public GameObject circle;
    private bool isDrag;
    public float maxRotationAngle = 55f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CircleSet();
        }
    }

    private void Shoot()
    {
        if(circle !=  null && ReloadManager.Instance.reloadExit == true)
        {
            circle.GetComponent<CircleCollider2D>().enabled = true;
            circle.AddComponent<CircleMove>().ShootStart(transform.up);
            CircleSet();
        }
    }

    public void CircleSet()
    {
        circle = ReloadManager.Instance.GetShootCircle();
        circle.transform.position = shootPosition.position;
    }

    private void OnMouseDown()
    {
        isDrag = true;
    }

    private void OnMouseDrag()
    {
        if(isDrag)
        {
            Pull();
        }
    }

    private void Pull()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mouseDistance = mousePos - (Vector2)transform.position;
        float rotZ = Mathf.Atan2(mouseDistance.y, mouseDistance.x) * Mathf.Rad2Deg;

        if (rotZ + 90 > -maxRotationAngle && rotZ + 90 < maxRotationAngle)
        {
            transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(rotZ + 90, -maxRotationAngle, maxRotationAngle));
        }
    }

    private void OnMouseUp()
    {
        isDrag = false;
        Shoot();
    }
}
