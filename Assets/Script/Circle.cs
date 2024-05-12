using UnityEngine;

public enum ColorType { Red, Green, Blue }

public class Circle : MonoBehaviour
{
    public ColorType colorType;
    public int index;
    public bool isMove;

    public void PositionSet()
    {
        transform.position = Coordinates.Instance.GetCloseCoordinate(transform.position, this);
    }

    public void ReloadExit()
    {
        Destroy(GetComponent<Animator>());
        ReloadManager.Instance.ReloadExit();
    }
}
