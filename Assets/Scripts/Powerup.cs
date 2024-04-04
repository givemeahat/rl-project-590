using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour
{
    public float boostNum;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("collide");
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("bazinga");

            collision.gameObject.GetComponent<jankController>().SpeedBoost();
            Destroy(this.gameObject);
        }

        //call function in jankController 
        //delete self
    }
}
