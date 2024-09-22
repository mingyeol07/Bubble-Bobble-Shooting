using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private Transform arrow;

    public GameObject circle;
    private bool isDrag;
    public float maxRotationAngle = 55f;
    private float rotationZ;

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

        if (Input.GetKeyDown(KeyCode.W))
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
        bool left = Input.GetKey(KeyCode.Q);
        bool right = Input.GetKey(KeyCode.E);

        if (left)
        {
            rotationZ += Time.deltaTime * 100;
        }

        if(right)
        {
            rotationZ -= Time.deltaTime * 100;
        }
        rotationZ = Mathf.Clamp(rotationZ, -maxRotationAngle, maxRotationAngle);

        arrow.rotation = Quaternion.Euler(0, 0, rotationZ);
    }
}
