using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Ghost : MonoBehaviour
{
    [SerializeField] private float speed;
    private Vector2 moveDirection;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private GameObject closeEnemy;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(x, y).normalized;

        if (x < 0) spriteRenderer.flipX = true;
        else if (x > 0) spriteRenderer.flipX = false;
    }

    private void FixedUpdate()
    {
        rb.velocity = moveDirection * speed;
    }
}
