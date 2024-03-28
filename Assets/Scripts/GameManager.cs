using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject ghostScreen;
    public CinemachineVirtualCamera virtualCamera;
    
    [Header("SoulBox")]
    [SerializeField] private Image img_soulBox;
    [SerializeField] private Sprite[] img_soulBoxLevel;
    private int soulIndex;
    private int soulLevel;

    [Header("Player")]
    [SerializeField] private Sprite[] img_playerLevel;
    [SerializeField] private SpriteRenderer playerSprite;

    [SerializeField] private RectTransform cursorPos;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        Cursor.visible = false;
        soulLevel = 0;
        img_soulBox.sprite = img_soulBoxLevel[0];
        playerSprite.sprite = img_playerLevel[0];
    }

    private void Update()
    {
        CursorPos();

        img_soulBox.fillAmount = soulIndex / 10;
    }

    private void CursorPos()
    {
        Vector2 mousePos = Input.mousePosition;
        cursorPos.position = mousePos;
    }

    public void GetSoul()
    {
        soulIndex++;
        if(soulIndex >= 10)
        {
            soulIndex = 0;
            soulLevel++;
            img_soulBox.sprite = img_soulBoxLevel[soulLevel];
            playerSprite.sprite = img_playerLevel[soulLevel];
        }
    }
}