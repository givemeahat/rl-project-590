using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class GroundSpawner : MonoBehaviour
{
    public GameObject[] grassPrefabs;
    public bool isDone = false;
    public GameObject terrainParent;

    public Collider2D terrainTarget;

    public LayerMask grassMask;

    public void Awake()
    {
        grassMask = LayerMask.GetMask("Ground");
        GenerateGrass();
    }

    public void GenerateGrass()
    {
        Debug.Log("hello");

        float xPlusVal = Random.Range(.1f, 5);
        this.transform.position = new Vector3(this.transform.position.x + xPlusVal, this.transform.position.y);
        // Cast a ray straight down.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, -Vector2.up, Mathf.Infinity, grassMask);
        if (hit)
        {
            int _grassNum = Random.Range(0, grassPrefabs.Length - 1);
            GameObject _go = Instantiate(grassPrefabs[_grassNum], terrainParent.transform) as GameObject;
            _go.transform.position = new Vector3(hit.point.x, hit.point.y);

            //attempting to set Z axis rotation by the raycast's normal
            //_go.transform.rotation = new Quaternion (_go.transform.rotation.x, _go.transform.rotation.y, hit.normal.x, 0f);
            _go.transform.up = hit.normal;
            _go.transform.localPosition = new Vector3(_go.transform.localPosition.x, _go.transform.localPosition.y, 18f);
            Debug.Log("bazinga");
            GenerateGrass();
        }
    }

    public void Update()
    {
        if (!isDone)
        {
            GenerateGrass();
        }
    }
}
