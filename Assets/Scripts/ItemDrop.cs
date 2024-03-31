using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    public Transform playerBag; // 플레이어의 가방 UI 위치
    public float throwHeight = 2f; // 아이템이 던져질 높이
    public float throwDuration = 1.5f; // 던지는 동안의 시간
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
        // 포물선 운동 계산
        float currentTime = Time.time - startTime;
        float ratio = currentTime / throwDuration;
        Vector3 currentPos = Vector3.Lerp(startPos, endPos, ratio);
        currentPos.y += Mathf.Sin(ratio * Mathf.PI) * power;

        // 아이템 이동
        transform.position = currentPos;

        // 도착하면 파괴
        if (ratio >= 1)
        {
            Destroy(gameObject);
        }
    }
}
