using System.Collections;
using UnityEngine;

public enum ColorType { Blue, Green, Red }

public class Circle : MonoBehaviour
{
    public ColorType colorType;
    public Vector2Int myCoordinate;
    private Animator animator;
    [SerializeField] private GameObject enemy;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetPosition()
    {
        transform.position = CoordinateManager.Instance.GetCloseCoordinatePos(transform.position, this);
    }

    public void CheckColor()
    {
        CoordinateManager.Instance.CheckCloseCoordinate(this);
    }

    public void Fall()
    {
        animator.enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        if(gameObject.GetComponent<Rigidbody2D>() == null )
        {
            gameObject.AddComponent<Rigidbody2D>().gravityScale = 1.0f;
        }
        else
        {
            gameObject.GetComponent<Rigidbody2D>().gravityScale = 1.0f;
        }
        
        StartCoroutine(FallExit());
    }

    IEnumerator FallExit()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    public void Boom()
    {
        animator = GetComponent<Animator>();
        animator.enabled = true;
        animator.applyRootMotion = true;
        animator.SetTrigger("Boom");

        Instantiate(enemy, transform.position, Quaternion.identity);
    }

    private void BoomAnimExit()
    {
        Destroy(gameObject);
    }

    public void ReloadExit()
    {
        ReloadManager.Instance.ReloadExit();
    }
}
