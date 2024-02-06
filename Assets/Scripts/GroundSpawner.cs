using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class GroundSpawner : MonoBehaviour
{
    public GameObject grassPrefab;
    public bool isDone = false;
    public GameObject terrainParent;

    public void Start()
    {
        GenerateGrass();
    }

    public void GenerateGrass()
    {
        Debug.Log("hello");
        float xPlusVal = Random.Range(10, 25);
        this.transform.position = new Vector3(this.transform.position.x + xPlusVal, this.transform.position.y);
        // Cast a ray straight down.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up);
        if (!hit)
        {
            isDone = true;
            GenerateGrass();
            return;
        }
        if (hit.collider.tag == "Ground")
        {
            GameObject _go = Instantiate(grassPrefab, terrainParent.transform) as GameObject;
            _go.transform.position = new Vector3(hit.point.x, hit.point.y);
            Debug.Log("bazinga");
            GenerateGrass();
        }
    }

    public void Update()
    {
        //if (!isDone) GenerateGrass();
    }
}
