using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public void Ghosting(Transform player)
    {
        transform.position = player.position;
    }
}
