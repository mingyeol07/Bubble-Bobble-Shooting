using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private Transform arrow;

    public GameObject circle;
    private bool isDrag;
    public float maxRotationAngle = 55f;

    private void Start()
    {
        StartCoroutine(StartSetCircle());
    }

    private IEnumerator StartSetCircle()
    {
        yield return new WaitForSeconds(0.5f);
        CircleSet();
    }

    private void Update()
    {
        Anim();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if(circle != null && ReloadManager.Instance.reloadExit == true)
        {
            circle.GetComponent<CircleCollider2D>().enabled = true;
            circle.GetComponent<Animator>().enabled = false;
            circle.transform.parent = null;
          
            circle.AddComponent<CircleMove>().ShootStart(arrow.transform.up);
            
            CircleSet();
        }
    }

    public void CircleSet()
    {
        circle = ReloadManager.Instance.GetShootCircle();
    }

    private void Anim()
    {
        float hor = Input.GetAxisRaw("Horizontal");

        float rotZ = Mathf.Atan2(arrow.transform.up.y, arrow.transform.up.x) * Mathf.Rad2Deg + 90 * hor;

        if (rotZ > -maxRotationAngle && rotZ < maxRotationAngle)
        {
            arrow.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(rotZ, -maxRotationAngle, maxRotationAngle));
        }
    }
}
