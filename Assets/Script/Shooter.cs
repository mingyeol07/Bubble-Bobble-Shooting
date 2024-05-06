using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [Header("Bullet")]
    [SerializeField] private GameObject bullet;

    [Header("Aim")]
    [SerializeField] private Transform shotPosition;

    public float maxRotationAngle = 55f;
    public float minRotationAngle = -55f;
    public float rotateSpeed = 1f;

    private float currentRotation = 0f;

    private void Update()
    {
        Aim();
        Shoot();
    }

    private void Aim()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (currentRotation > minRotationAngle)
            {
                currentRotation -= rotateSpeed;
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (currentRotation < maxRotationAngle)
            {
                currentRotation += rotateSpeed;
            }
        }

        transform.rotation = Quaternion.Euler(0f, 0f, currentRotation);
    }

    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Instantiate(bullet, shotPosition.position, transform.rotation);
        }
    }
}
