using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMove : MonoBehaviour
{
    private float speed = 10;
    private Rigidbody2D rigid;
    private Vector2 velocity;
    private Circle circle;
    private bool isStop = false;

    private void Start()
    {
        circle = GetComponent<Circle>();
        rigid = GetComponent<Rigidbody2D>();
    }

    public void ShootStart(Vector2 moveVec)
    {
        circle = GetComponent<Circle>();
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = moveVec * speed;
    }

    void Update()
    {
       if(rigid != null)  velocity = rigid.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Circle") || collision.gameObject.CompareTag("Ceiling"))
        {
            if (!isStop)
            {
                isStop = true;
                StartCoroutine(Co_ColorCheck());
            }
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            rigid.velocity = Vector2.Reflect(velocity, collision.contacts[0].normal);
        }
    }

    private IEnumerator Co_ColorCheck()
    {
        yield return new WaitForSeconds(0.05f);
        SetPosition();
    }

    private void SetPosition()
    {
        rigid.velocity = Vector2.zero;
        DestroyComponent();
        circle.SetPosition();
        circle.CheckColor();
    }

    private void DestroyComponent()
    {
        Destroy(GetComponent<CircleMove>());
        Destroy(GetComponent<Rigidbody2D>());
    }
}
