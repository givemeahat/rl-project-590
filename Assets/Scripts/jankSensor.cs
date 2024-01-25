using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jankSensor : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Hunter")
            Debug.Log("nicki minaj voice SOUND THE ALARM");
        this.GetComponent<CircleCollider2D>().enabled = false;
    }
}
