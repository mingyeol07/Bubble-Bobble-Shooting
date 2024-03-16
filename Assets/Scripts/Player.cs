using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
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
        Moving();
        Shot();
        Ghosting();

    }

    private void Moving()
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

    private void Ghosting()
    { 
        
    }
}