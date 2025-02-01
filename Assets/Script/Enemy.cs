// # Systems
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;


// # Unity
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rigid;
    private float speed = 3;
    private bool isFly = true;
    private bool isUnNoramlGame = false;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        if (!isUnNoramlGame)
        {
            Debug.Log("DD");
            GetComponent<CircleCollider2D>().isTrigger = true;
            Destroy(this.gameObject, 4);
            return;
        }

        isFly = true;
        int ranDir = Random.Range(-2, 1);
        transform.eulerAngles = ranDir < 0 ? new Vector3(0, transform.eulerAngles.y - 180, 0) : Vector3.zero;
    }

    private void Update()
    {
        if(!isFly) rigid.velocity = transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isUnNoramlGame && collision.gameObject.CompareTag("Ground"))
        {
            isFly = false;
        }

        if(isUnNoramlGame && collision.gameObject.CompareTag("Wall"))
        {
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y - 180, 0);
        }
    }

    public void FlyAway()
    {
        GetComponent<Collider2D>().enabled = false;
        isFly = true;

        // ���� �޸��� �������� ���ư����� ��
        Vector2 launchDirection = ((Vector2)transform.right + Vector2.up).normalized; // ���� �̵� ���� + ���� ����
        float launchForce = 10f; // ���ư��� ���� ũ��

        rigid.velocity = launchDirection * launchForce;

        Destroy(this.gameObject, 3f);
    }
}
