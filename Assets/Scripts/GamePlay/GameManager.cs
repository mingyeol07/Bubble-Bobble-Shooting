using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public CinemachineVirtualCamera virtualCamera;
    public Transform soulBoxPos;

    [SerializeField] private RectTransform cursorPos;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        CursorPos();
    }

    private void CursorPos()
    {
        Vector2 mousePos = Input.mousePosition;
        cursorPos.position = mousePos;
    }
}