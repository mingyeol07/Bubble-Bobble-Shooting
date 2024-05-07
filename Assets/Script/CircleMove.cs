using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMove : MonoBehaviour
{
    [SerializeField] private float speed;
    Rigidbody2D rigid;
    Vector2 velocity;
    Circle circle;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = transform.up * speed;
        circle = GetComponent<Circle>();
    }

    private void Update()
    {
        velocity = rigid.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Circle") || collision.gameObject.CompareTag("Ceiling"))
        {
            Debug.Log("stop");
            rigid.velocity = Vector2.zero;
            Destroy(GetComponent<CircleMove>());
            Destroy(GetComponent<Rigidbody2D>());
            circle.PositionSet();
            
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            rigid.velocity = Vector2.Reflect(velocity, collision.contacts[0].normal);
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(velocity.x, velocity.y) * Mathf.Rad2Deg, transform.forward);
        }
    }
}
