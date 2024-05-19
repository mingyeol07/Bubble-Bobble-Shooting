using System.Collections;
using UnityEngine;

public enum ColorType { Blue, Green, Red }

public class Circle : MonoBehaviour
{
    public ColorType colorType;
    public Vector2Int myCoordinate;
    private Animator animator;

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

    public void Boom()
    {
        animator = GetComponent<Animator>();
        animator.enabled = true;
        animator.applyRootMotion = true;
        animator.SetTrigger("Boom");
    }

    public void Fall()
    {
        animator.enabled = false;
        gameObject.AddComponent<Rigidbody2D>().gravityScale = 1.0f;

        StartCoroutine(FallExit());
    }

    IEnumerator FallExit()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    private void BoomAnimExit()
    {
        Destroy(gameObject);
    }

    public void ReloadExit()
    {
        animator.enabled = false;
        ReloadManager.Instance.ReloadExit();
    }
}
