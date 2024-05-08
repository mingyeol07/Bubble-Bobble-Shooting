using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMove : MonoBehaviour
{
    [SerializeField] private float speed;
    private Rigidbody2D rigid;
    private Vector2 velocity;

    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = transform.up * speed;
    }

    private void Update()
    {
        velocity = rigid.velocity;
    }

    public void PositionSet()
    {
        transform.position = Coordinates.Instance.GetCloseCoordinate(transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Circle") || collision.gameObject.CompareTag("Ceiling"))
        {
            rigid.velocity = Vector2.zero;
            Destroy(GetComponent<CircleMove>());
            Destroy(GetComponent<Rigidbody2D>());
            PositionSet();            
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            rigid.velocity = Vector2.Reflect(velocity, collision.contacts[0].normal);
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(velocity.x, velocity.y) * Mathf.Rad2Deg, transform.forward);
        }
    }
}
