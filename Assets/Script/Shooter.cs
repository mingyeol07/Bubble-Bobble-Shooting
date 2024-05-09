using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private GameObject bullet;

    [Header("Aim")]
    [SerializeField] private Transform shotPosition;

    private CircleMove circle;

    private bool isDrag;

    public float maxRotationAngle = 55f;

    private void Shoot()
    {
       
         Instantiate(bullet, shotPosition.position, transform.rotation);
        bullet.GetComponent<CircleMove>().StartShoot();
    }

    private void OnMouseDown()
    {
        isDrag = true;
    }

    private void OnMouseDrag()
    {
        if(isDrag)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mouseDistance = mousePos - (Vector2)transform.position;
            float rotZ = Mathf.Atan2(mouseDistance.y, mouseDistance.x) * Mathf.Rad2Deg;

            if(rotZ + 90 > -maxRotationAngle && rotZ + 90 < maxRotationAngle)
            {
                transform.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(rotZ + 90, -maxRotationAngle, maxRotationAngle));
            }
           
        }
    }

    private void OnMouseUp()
    {
        isDrag = false;
        Shoot();
    }
}
