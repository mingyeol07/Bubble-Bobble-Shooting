using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private int maxHp;
    private int curHp;

    private Rigidbody2D rigid;
    private SpriteRenderer spriteRenderer;

    public float amplitude = 1f; // ÁøÆø

    private float angle = 0f;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigid = GetComponent<Rigidbody2D>();
        curHp = maxHp;
    }

    private void FixedUpdate()
    {
        
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
