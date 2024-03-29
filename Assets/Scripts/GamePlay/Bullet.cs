using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private GameObject effect;
    public int damage = 1;

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
            Destroy(Instantiate(effect, transform.position, Quaternion.identity), 0.5f);
            Destroy(gameObject);
        }
    }
}
