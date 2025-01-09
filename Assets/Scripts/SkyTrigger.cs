using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyTrigger : MonoBehaviour
{
    public GameObject objSpawner;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Trigger Change");
    }

    public void TriggerObjectGen()
    {
        objSpawner.SetActive(true);
    }
}
