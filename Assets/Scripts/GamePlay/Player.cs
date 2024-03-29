using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// shot, move, ghost
public class Player : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed;

    [Header("Ghost")]
    [SerializeField] private bool ghosting;
    [SerializeField] private GameObject ghostSoul;
    [SerializeField] private Image ghostGauge;
    [SerializeField] private float gaugeValueSecond;
    private GameObject enemy;

    [Header("Shot")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gun;
    [SerializeField] private Transform shotPoint;

    private Rigidbody2D rigid;
    private SpriteRenderer myRenderer;
    private Vector2 moveDirection;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        myRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        ProcessInput();
        Ghosting();

        if (Input.GetKey(KeyCode.Mouse1)) ghosting = true;
        else ghosting = false;
    }

    private void FixedUpdate()
    {
        if(!ghosting) { Move(); }
    }

    private void ProcessInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector2(x, y).normalized;

        if (Input.GetKeyDown(KeyCode.Mouse0) && ghosting == false)
        {
            Shot();
        }

        //Ghosting
        

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            ghosting = true;
            rigid.velocity = Vector3.zero;
        }

        if (Input.GetKeyUp(KeyCode.Mouse1)) 
        {
            ghosting = false;
        }
    }

    private void Move()
    {
        rigid.velocity = moveDirection * moveSpeed;
    }

    private void Shot()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = mousePos - (Vector2)gun.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        gun.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        GameObject obj = Instantiate(bullet, shotPoint.position, Quaternion.AngleAxis(angle, Vector3.forward));
        obj.GetComponent<Bullet>().Moving(mousePos - (Vector2)shotPoint.position);
    }

    private void Ghosting()
    { 
        if (ghosting)
        {
            ghostGauge.fillAmount -= Time.unscaledDeltaTime / 3;
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(transform.position);
        }
        else
        {
            ghostGauge.fillAmount += Time.unscaledDeltaTime / 10;
        }
    }

    public void SetEnemy(GameObject go)
    {
        enemy = go;
    }
}