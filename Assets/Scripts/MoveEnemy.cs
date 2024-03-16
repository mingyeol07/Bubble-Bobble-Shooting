using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float upDownMoveSpeed;
    [SerializeField] private float moveCool;
    [SerializeField] private int maxHp;
    private int curHp;

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;

    private bool up;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        curHp = maxHp;

        StartCoroutine(MoveUpDown());
    }

    private void FixedUpdate()
    {
        if(up)
        {
            Vector3 targetPos = new Vector3(transform.position.x + moveSpeed, transform.position.y + upDownMoveSpeed, 0);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * upDownMoveSpeed);
        }
        else
        {
            Vector3 targetPos = new Vector3(transform.position.x + moveSpeed, transform.position.y - upDownMoveSpeed, 0);
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * upDownMoveSpeed);
        }
    }

    private IEnumerator MoveUpDown()
    {
        if(up)up = false;
        else up = true;
        yield return new WaitForSeconds(moveCool);
        StartCoroutine(MoveUpDown());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            StartCoroutine(HpDown());
        }
    }

    private IEnumerator HpDown()
    {
        curHp -= 1;
        if (curHp <= 0)
        {
            Destroy(gameObject);
        }

        spriteRenderer.color = new Color(1f, 1f, 1f, 0.4f);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
    }
}
