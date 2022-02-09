using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class switchPriority : MonoBehaviour
{
    public CinemachineVirtualCamera vcam1; //original vcam
    public CinemachineVirtualCamera vcam2; //vcam to switch to
    public int priority = 10;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag.Equals("Player")) {
            vcam1.Priority = 0;
            vcam2.Priority = priority;
        }
    }
}
