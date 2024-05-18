using System.Collections.Generic;
using UnityEngine;

public enum ColorType { Red, Green, Blue }

public class Circle : MonoBehaviour
{
    public ColorType colorType;
  //  public CoordinateData myCoordinate;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PositionSet()
    {
        transform.position = CoordinateManager.Instance.GetCloseCoordinatePos(transform.position, this);
    }

    public void CheckColor()
    {
        //CoordinateManager.Instance.CheckCloseCoordinate(transform.position.x, transform.posiiton.y, this);
        //CoordinateManager.Instance.CheckForSameColorCircles(myCoordinate, colorType);
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
