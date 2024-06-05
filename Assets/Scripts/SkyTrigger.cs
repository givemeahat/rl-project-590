using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyTrigger : MonoBehaviour
{
    public GameObject objSpawner;
    public void TriggerObjectGen()
    {
        objSpawner.SetActive(true);
    }
}
