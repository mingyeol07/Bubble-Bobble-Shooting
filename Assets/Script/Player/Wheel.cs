// # Systems
using System.Collections;
using System.Collections.Generic;

// # Unity
using UnityEngine;

public class Wheel : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private Animator wheelAnimator;
    [SerializeField] private float moveSpeed;
    private float hor;

    private void Update()
    {
        hor = Input.GetAxisRaw("Horizontal");
        wheelAnimator.SetInteger("Move", (int)hor);
        rigid.velocity = Vector2.right * hor * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy") && (int)hor != 0)
        {
            collision.GetComponent<Enemy>().FlyAway();
        }
    }
}
