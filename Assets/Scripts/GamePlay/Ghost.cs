using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public Player player;
    public GameObject enemy;

    private void Awake()
    {
        player = GetComponentInParent<Player>();
    }

    private void Start()
    {
        enemy = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            enemy = collision.gameObject;
            enemy.GetComponent<SpriteRenderer>().color = Color.white;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            enemy = null;
            
        }
    }

    private void OnDisable()
    {
        if(enemy != null)
        {
            player.SetEnemy(enemy);
        }
    }
}
