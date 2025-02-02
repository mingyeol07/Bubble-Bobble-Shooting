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

    public IEnumerator StartSetCircle()
    {
        yield return new WaitForSeconds(1f);
        CircleSet();
    }
    
    public void DestroyCircle()
    {
        if (circle == null) return;
        Destroy(circle);
    }

    private void Update()
    {
        Anim();

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
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
        bool left = Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow);
        bool right = Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.RightArrow);

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
