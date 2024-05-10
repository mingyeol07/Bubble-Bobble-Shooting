using UnityEngine;

public enum ColorType { Red, Green, Blue }

public class Circle : MonoBehaviour
{
    public ColorType color;

    public void CloseSameColor()
    {
        CirclePoolManager.Instance.DeSpawn(color, gameObject);
    }
}
