using UnityEngine;

public enum ColorType { Red, Green, Blue }

public class Circle : MonoBehaviour
{
    public ColorType colorType;
    public int index;
    public bool isMove;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PositionSet()
    {
        transform.position = Coordinates.Instance.GetCloseCoordinate(transform.position, this);
    }

    public void CheckColor()
    {
        Coordinates.Instance.CheckForSameColorCircles(index, colorType);
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
