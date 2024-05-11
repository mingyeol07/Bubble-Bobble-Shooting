using UnityEngine;

public enum ColorType { Red, Green, Blue }

public class Circle : MonoBehaviour
{
    public ColorType colorType;
    public int number;

    public void DeSpawnCircle()
    {
        if (gameObject != null) CirclePoolManager.Instance.DeSpawn(colorType, gameObject);
    }

    public void PositionSet()
    {
        if (gameObject != null) transform.position = Coordinates.Instance.GetCloseCoordinate(transform.position, this);
    }
}
