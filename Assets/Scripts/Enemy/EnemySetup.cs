using System.Collections;
using DG.Tweening;
using UnityEngine;

public class EnemySetup : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private Transform enemySprite;
    [SerializeField] private GameObject soul;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = enemySprite.GetComponent<SpriteRenderer>();
    }

    public void SpawnMove(Transform pos)
    {
        transform.DOMove(pos.position, 1f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            StartCoroutine(HpDown(collision.GetComponent<Bullet>().damage));
        }
    }

    private IEnumerator HpDown(int value)
    {
        hp -= value;
        if (hp <= 0)
        {
            Die();
        }

        Color color = spriteRenderer.color;

        spriteRenderer.color = new Color(color.r, color.g, color.b, 0.4f);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = new Color(color.r, color.g, color.b, 1f);
    }

    public void Die()
    {
        Destroy(gameObject);
        Instantiate(soul, transform.position, Quaternion.identity);
    }
}
