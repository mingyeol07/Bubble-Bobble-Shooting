using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private GameObject[] prefabs = null;
    public List<GameObject>[] pools = null;

    private void Awake()
    {
        GameFacadeManager.poolManager = this;

        pools = new List<GameObject>[prefabs.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    /// <summary>
    /// 소환할 몬스터의 아이디넘버
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public GameObject Get(int index)
    {
        GameObject select = null;

        foreach (GameObject gameObject in pools[index])
        {
            if (!gameObject.activeSelf)
            {
                select = gameObject;
                select.SetActive(true);
                break;
            }

            if(!select)
            {
                select = Instantiate(prefabs[index], transform);
                pools[index].Add(select);
            }
        }

        return select;
    }
}
