using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using System;

public class terrainGen : MonoBehaviour
{
    // Start is called before the first frame update
    public SpriteShapeController shape;
    public int scale;

    public int pointCount ;

    public int heightRange;
    public float heightLow;
    public float heightHigh;


    void Start()
    {
        shape = GetComponent<SpriteShapeController>();
        float distanceBetweenPoints = (float)scale/pointCount;
        shape.spline.SetPosition(0, shape.spline.GetPosition(0)-Vector3.right*scale/2);
        shape.spline.SetPosition(1, shape.spline.GetPosition(1)-Vector3.right*scale/2);

        shape.spline.SetPosition(2, shape.spline.GetPosition(2)+Vector3.right*scale/2);
        shape.spline.SetPosition(3, shape.spline.GetPosition(3)+Vector3.right*scale/2);

        for (int i = 0; i < pointCount; i++){ //adding a total of pointCount points to the middle of the 4 existing points
          float xPos = shape.spline.GetPosition(i+1).x + distanceBetweenPoints; 
          //can just check for odd or even of i, and then have a range of height to add / minus to y in the next line
          if (i%2 == 0){
            shape.spline.InsertPointAt(i+2, new Vector3(xPos, heightRange + 10*Mathf.PerlinNoise(i*UnityEngine.Random.Range(heightLow, heightHigh),0)));
          }
          else{
            shape.spline.InsertPointAt(i+2, new Vector3(xPos, -heightRange + 10*Mathf.PerlinNoise(i*UnityEngine.Random.Range(heightLow, heightHigh),0)));
          }
        }

        for (int i = 0; i < pointCount; i++){
          shape.spline.SetTangentMode(i+2,ShapeTangentMode.Continuous);
          shape.spline.SetLeftTangent(i+2,new Vector3(-2,0,0));
          shape.spline.SetRightTangent(i+2,new Vector3(2,0,0));
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
