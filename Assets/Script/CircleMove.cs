using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMove : MonoBehaviour
{
    private float speed = 10;
    private Rigidbody2D rigid;
    private Vector2 velocity;
    private Circle cricle;
    private bool isMove;

    void Start()
    {
        cricle = GetComponent<Circle>();
        rigid = GetComponent<Rigidbody2D>();
    }

    public void StartShoot()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = transform.up * speed;
    }

    void Update()
    {
        velocity = rigid.velocity;
    }

    public void PositionSet()
    {
        transform.position = Coordinates.Instance.GetCloseCoordinate(transform.position, cricle);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Circle") || collision.gameObject.CompareTag("Ceiling"))
        {
            StartCoroutine(Co_SetPosition());
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            rigid.velocity = Vector2.Reflect(velocity, collision.contacts[0].normal);
            transform.rotation = Quaternion.AngleAxis(Mathf.Atan2(velocity.x, velocity.y) * Mathf.Rad2Deg, transform.forward);
        }
    }

    private IEnumerator Co_SetPosition()
    {
        yield return new WaitForSeconds(0.05f);
        rigid.velocity = Vector2.zero;
        Destroy(GetComponent<CircleMove>());
        Destroy(GetComponent<Rigidbody2D>());
        PositionSet();
    }

}
