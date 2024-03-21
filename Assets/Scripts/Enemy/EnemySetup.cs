using System.Collections;
using DG.Tweening;
using UnityEngine;

public class EnemySetup : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private Transform enemySprite;
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
            StartCoroutine(HpDown());
        }
    }

    private IEnumerator HpDown()
    {
        Color color = spriteRenderer.color;
        hp -= 1;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }

        spriteRenderer.color = new Color(color.r, color.g, color.b, 0.4f);
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = new Color(color.r, color.g, color.b, 1f);
    }
}
