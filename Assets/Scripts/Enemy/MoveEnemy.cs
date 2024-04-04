using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private GameObject warningImage;
    private Transform player;
    private float stoppingDistance = 3f;
    private bool isAttack;
    private Rigidbody2D rb;
   

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer > stoppingDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
        else if (!isAttack)
        {
            isAttack = true;
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        warningImage.SetActive(true);
        yield return new WaitForSeconds(1f);
        warningImage.SetActive(false);

        player = GameObject.FindWithTag("Player").transform;

        if (player != null) // �÷��̾ �����ϸ�
        {
            // �÷��̾� ���� ���
            Vector2 moveDirection = (player.position - transform.position).normalized;

            // ���� �÷��̾� �������� �̵�
            rb.velocity = new Vector2(moveDirection.x * speed, moveDirection.y * speed);
        }
    }
}
