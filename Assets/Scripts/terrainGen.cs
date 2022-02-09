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
    
    public float minHeightDifference;

    public float heightRange;
    private float randomLow = 10;
    private float randomHigh = 15;
    private float distanceBetweenPoints;


    void Start()
    {
        shape = GetComponent<SpriteShapeController>();
        distanceBetweenPoints = (float)scale/pointCount;

        shape.spline.SetPosition(0, shape.spline.GetPosition(0)-Vector3.right*scale/2- new Vector3(0,10,0));
        shape.spline.SetPosition(1, shape.spline.GetPosition(1)-Vector3.right*scale/2);

        shape.spline.SetPosition(2, shape.spline.GetPosition(2)+Vector3.right*scale/2);
        shape.spline.SetPosition(3, shape.spline.GetPosition(3)+Vector3.right*scale/2- new Vector3(0,10,0));
        
        /*was getting a compile time bug where spawn position can't see that we are adding 300 points... 
        can get around this by initializing all the points and then essentially reset their posiiton at 
        the next generation; if do this method, no need to delete points, can use the same generation method
        */

        //initalize all the points we want to add
        for(int i = 0; i<pointCount; i++){
          float xPos = shape.spline.GetPosition(i+1).x + distanceBetweenPoints;
          shape.spline.InsertPointAt(i+2, new Vector3(xPos, shape.spline.GetPosition(1).y,0));
        }
    }
  
    /* This function randomly resets the terrain!
    */
    public void generateTerrain(){
        for (int i = 0; i < pointCount; i++){ //adding a total of pointCount points to the middle of the 4 existing points
          float xPos = shape.spline.GetPosition(i+1).x + distanceBetweenPoints; 
          float yPos = 10 * Mathf.PerlinNoise(i*UnityEngine.Random.Range(randomLow, randomHigh),0);

          float pYPos = shape.spline.GetPosition(i+1).y;
          //makes sure there's no "flat" hills
          if (Mathf.Abs(pYPos - yPos) < minHeightDifference){
            heightRange =  minHeightDifference;
          }

          if (i%2 == 0){
            shape.spline.SetPosition(i+2, new Vector3(xPos, yPos + heightRange, 0));
          }
          else{
            shape.spline.SetPosition(i+2, new Vector3(xPos, yPos - heightRange, 0));
          }
        }

        for (int i = 0; i < pointCount; i++){
          shape.spline.SetTangentMode(i+2,ShapeTangentMode.Continuous);
          shape.spline.SetLeftTangent(i+2,new Vector3(-2,0,0));
          shape.spline.SetRightTangent(i+2,new Vector3(2,0,0));
        }
    }

    // public void resetTerrain(){
    //   shape.spline
    // }

    // Update is called once per frame
    void Update()
    {

    }
}
