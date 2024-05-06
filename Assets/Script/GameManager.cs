using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }
    public List<GameObject> targets = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
    }

    public void RemoveTarget(GameObject target)
    {
        targets.Remove(target);
        if(targets.Count <= 0 ) {
            // GameClear
        }
    }

    public void ShootBullet()
    {

    }
}
