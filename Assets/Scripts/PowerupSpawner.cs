using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    public GameObject[] powerUps;
    public bool isDone = false;
    public GameObject terrainParent;

    public Collider2D terrainTarget;

    public LayerMask grassMask;

    public void Awake()
    {
        grassMask = LayerMask.GetMask("Ground");
        GeneratePWRUps();
    }

    public void GeneratePWRUps()
    {
        float xPlusVal = Random.Range(10,1000);
        this.transform.position = new Vector3(this.transform.position.x + xPlusVal, this.transform.position.y);
        // Cast a ray straight down.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, Mathf.Infinity, grassMask);
        if (hit)
        {
            int _pwrNum = Random.Range(0, powerUps.Length - 1);
            GameObject _go = Instantiate(powerUps[_pwrNum], terrainParent.transform) as GameObject;
            _go.transform.position = new Vector3(hit.point.x, hit.point.y);

            //attempting to set Z axis rotation by the raycast's normal
            //_go.transform.rotation = new Quaternion (_go.transform.rotation.x, _go.transform.rotation.y, hit.normal.x, 0f);
            _go.transform.up = hit.normal;
            _go.transform.localPosition = new Vector3(_go.transform.localPosition.x, _go.transform.localPosition.y + .5f, -8f);
            GeneratePWRUps();
        }
    }

    public void Update()
    {
        if (!isDone)
        {
            GeneratePWRUps();
        }
    }
}
