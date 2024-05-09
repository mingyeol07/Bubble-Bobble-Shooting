using UnityEngine;

public enum ColorType { Red, Green, Blue }

public class Circle : MonoBehaviour
{
    public ColorType colorType;

    public void CloseSameColor()
    {
        Destroy(gameObject);
    }
}
