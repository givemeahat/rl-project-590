using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter_Group : MonoBehaviour
{
    public float speed = 10f;

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.right * speed * Time.deltaTime;
    }


}
