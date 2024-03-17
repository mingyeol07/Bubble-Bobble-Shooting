using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    private void Start()
    {
        Destroy(gameObject, 2f);
    }

    public void Moving(Vector2 direction)
    {
        Vector2 dir = direction;
        GetComponent<Rigidbody2D>().velocity = dir.normalized * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
