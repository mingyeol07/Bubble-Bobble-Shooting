using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public Transform playerBag; // �÷��̾��� ���� UI ��ġ
    public float throwHeight = 2f; // �������� ������ ����
    public float throwDuration = 1.5f; // ������ ������ �ð�
    private float power;

    private Vector3 startPos;
    private Vector3 endPos;
    private float startTime;

    private void Start()
    {
        playerBag = GameManager.instance.soulBoxPos;
        startPos = transform.position;
        endPos = playerBag.position;
        startTime = Time.time;
    }

    private void OnEnable()
    {
        power = Random.Range(1, throwHeight);
    }

    private void Update()
    {
        // ������ � ���
        float currentTime = Time.time - startTime;
        float ratio = currentTime / throwDuration;
        Vector3 currentPos = Vector3.Lerp(startPos, endPos, ratio);
        currentPos.y += Mathf.Sin(ratio * Mathf.PI) * power;

        // ������ �̵�
        transform.position = currentPos;

        // �����ϸ� �ı�
        if (ratio >= 1)
        {
            Destroy(gameObject);
        }
    }
}
