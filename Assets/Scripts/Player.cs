using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed;

    [Header("Ghost")]
    [SerializeField] private bool ghosting;
    [SerializeField] private GameObject ghostSoul;
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

        if (Input.GetKeyDown(KeyCode.LeftShift) && ghosting == false)
        {
            StartCoroutine(GhostStart());
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
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    public void SetEnemy(GameObject go)
    {
        enemy = go;
    }

    public IEnumerator GhostStart()
    {
        ghostSoul.SetActive(true);
        ghostSoul.transform.position = this.transform.position;
        ghosting = true;
        //GameManager.instance.ghostScreen.SetActive(true);
        GameManager.instance.virtualCamera.Follow = ghostSoul.transform;
        myRenderer.color = Color.blue;

        rigid.velocity = Vector3.zero;

        yield return new WaitForSeconds(1f);

        GameManager.instance.virtualCamera.Follow = this.transform;
        ghostSoul.SetActive(false);
        //GameManager.instance.ghostScreen.SetActive(false);
        myRenderer.color = Color.white;

        ghosting = false;
    }
}