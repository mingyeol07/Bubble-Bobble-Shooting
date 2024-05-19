using System.Collections.Generic;
using UnityEngine;

public enum ColorType { Blue, Green, Red }

public class Circle : MonoBehaviour
{
    public ColorType colorType;
    public Vector2Int myCoordinate;
    private Animator animator;

    private void Awake()
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
        animator.enabled = true;
        animator.applyRootMotion = true;
        animator.SetTrigger("Boom");
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
