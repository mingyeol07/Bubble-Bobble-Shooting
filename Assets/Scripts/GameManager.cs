using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject ghostScreen;
    public CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        instance = this;
    }
}