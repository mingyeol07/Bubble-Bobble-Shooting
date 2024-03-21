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
            float closestDistance = Mathf.Infinity; // 가장 가까운 거리를 저장하는 변수를 무한대로 초기화

            GameObject previousCloseEnemy = closeEnemy; // 이전 프레임에서 가장 가까웠던 적을 기억
            closeEnemy = null; // 가장 가까운 적을 찾기 전에 null로 초기화

            for (int i = 0; i < enemys.Length; i++)
            {
                float distance = Vector3.Distance(transform.position, enemys[i].transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closeEnemy = enemys[i];
                }
            }

            // 이전에 가장 가까웠던 적의 색상을 원래대로 돌림
            if (previousCloseEnemy != null && previousCloseEnemy != closeEnemy)
            {
                previousCloseEnemy.GetComponentInChildren<SpriteRenderer>().color = Color.white; // 원래 색으로 돌리는 코드, 실제 색에 맞게 조정 필요
            }

            // 새로 가장 가까운 적을 파란색으로 변경
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

        // closeEnemy가 유효한 경우에만 위치 변경
        if (closeEnemy != null)
        {
            transform.position = closeEnemy.transform.position; // 자식 오브젝트가 아닌 적 자체의 위치로 이동
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