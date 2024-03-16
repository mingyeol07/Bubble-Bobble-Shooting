using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed;

    [Header("Ghost")]
    private bool ghosting;

    [Header("Shot")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gun;
    [SerializeField] private Transform shotPoint;

    private Rigidbody2D rigid;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Move();
        Shot();
        Ghost();

    }

    private void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        rigid.velocity = new Vector2(x, y).normalized * moveSpeed;
    }

    private void Shot()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousePos - (Vector2)gun.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        gun.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
           GameObject obj = Instantiate(bullet, transform);
           obj.GetComponent<Bullet>().Moving(mousePos - (Vector2)shotPoint.position);

        }
    }

    private void Ghost()
    { 
        ghosting = Input.GetKey(KeyCode.Space);
        
        if (ghosting)
        {
            Time.timeScale = 0.5f;
            GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");

            
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}