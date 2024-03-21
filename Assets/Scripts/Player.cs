using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Move")]
    [SerializeField] private float moveSpeed;

    [Header("Ghost")]
    [SerializeField] private bool ghosting;
    [SerializeField] private float ghostSpeed;
    [SerializeField] private GameObject ghostSoul;
    private GameObject closeEnemy;

    [Header("Shot")]
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gun;
    [SerializeField] private Transform shotPoint;

    private Rigidbody2D rigid;
    private Vector2 moveDirection;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        ProcessInput();
        Ghosting();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void ProcessInput()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        moveDirection = new Vector2(x, y).normalized;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Shot();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && ghosting == false) StartCoroutine(GhostStart());
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
            GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
            float closestDistance = Mathf.Infinity; // ���� ����� �Ÿ��� �����ϴ� ������ ���Ѵ�� �ʱ�ȭ

            GameObject previousCloseEnemy = closeEnemy; // ���� �����ӿ��� ���� ������� ���� ���
            closeEnemy = null; // ���� ����� ���� ã�� ���� null�� �ʱ�ȭ

            for (int i = 0; i < enemys.Length; i++)
            {
                float distance = Vector3.Distance(transform.position, enemys[i].transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closeEnemy = enemys[i];
                }
            }

            // ������ ���� ������� ���� ������ ������� ����
            if (previousCloseEnemy != null && previousCloseEnemy != closeEnemy)
            {
                previousCloseEnemy.GetComponentInChildren<SpriteRenderer>().color = Color.white; // ���� ������ ������ �ڵ�, ���� ���� �°� ���� �ʿ�
            }

            // ���� ���� ����� ���� �Ķ������� ����
            if (closeEnemy != null)
            {
                closeEnemy.GetComponentInChildren<SpriteRenderer>().color = Color.blue;
            }
        }
    }

    private IEnumerator GhostStart()
    {
        GameManager.instance.ghostScreen.SetActive(true);
        float nomalSpeed = moveSpeed;
        moveSpeed = ghostSpeed;
        ghosting = true;

        yield return new WaitForSeconds(1f);

        GameManager.instance.ghostScreen.SetActive(false);
        moveSpeed = nomalSpeed;

        // closeEnemy�� ��ȿ�� ��쿡�� ��ġ ����
        if (closeEnemy != null)
        {
            transform.position = closeEnemy.transform.position; // �ڽ� ������Ʈ�� �ƴ� �� ��ü�� ��ġ�� �̵�
        }

        ghosting = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(ghosting == false)
        {
            
        }
    }
}