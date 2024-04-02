using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopEnemy : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject warningImage;
    [SerializeField] private Transform gun;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        StartCoroutine("Shot");
    }

    private IEnumerator Shot()
    {
        warningImage.SetActive(true);
        yield return new WaitForSeconds(1f);
        warningImage.SetActive(false);

        Vector2 playerPos = player.position;
        Vector2 dir = playerPos - (Vector2)gun.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        gun.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        for(int i =0; i < 3; i++)
        {
            GameObject obj = Instantiate(bullet, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
            obj.GetComponent<EnemyBullet>().Moving(playerPos - (Vector2)transform.position);
        }
    }
}