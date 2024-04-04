using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public Transform player; // �÷��̾��� ��ġ
    public float throwHeight = 2f; // �������� ������ ����
    public float throwDuration = 1.5f; // ������ ������ �ð�
    private float power;

    private Vector3 startPos;
    private Vector3 endPos;
    private float startTime;

    private void Start()
    {
        startPos = transform.position;
        startTime = Time.time;
    }

    private void OnEnable()
    {
        power = Random.Range(1, throwHeight);
    }

    private void Update()
    {
        player = GameManager.instance.player;
        endPos = player.position;

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
            LevelUpManager.instance.GetSoul();
            Destroy(gameObject);
        }
    }
}
