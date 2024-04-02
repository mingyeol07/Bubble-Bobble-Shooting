using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private GameObject effect;

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
        if (collision.CompareTag("Player"))
        {
            Destroy(Instantiate(effect, transform.position, Quaternion.identity), 0.5f);
            Destroy(gameObject);
        }
    }
}
