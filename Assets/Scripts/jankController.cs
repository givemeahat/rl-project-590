using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;



public class jankController : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody2D;

    [SerializeField] private float added_gravity;
    [SerializeField] private float original_gravity;

    [SerializeField] private float airDrag;

    private bool grounded;

    private float slopeDownAngle;
    private Vector2 slopeNormalPerpendicular;
    private float slopeSideAngle;
    private bool isOnSlope;
    private Vector2 colliderSize;

    private bool downhill;
    private bool uphill;

    public float min_xforce;

    [SerializeField] private bool reset = false;

    [SerializeField] private float slopeCheckDistance;

    [SerializeField] private terrainGen TerrainGen;
    private Vector3 splineReference; 



    private void Awake(){
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
		    colliderSize = GetComponents<Collider2D>()[0].bounds.extents;

    }
    private void Start()
    {
      // SpawnPosition();
    }

    // Update is called once per frame
    private void Update()
    {
        if(reset){
          // Reset();
          StartCoroutine(Reset());
          reset = false;
        }
        //Jank is now controllable by keyboard input in jankgent
        if(Input.GetButtonDown("Dive")){
           dive();
        }
        if(Input.GetButtonUp("Dive")){
            diveFalse();
        }

        SlopeCheck();


    }

    public void dive(){
        m_Rigidbody2D.gravityScale = added_gravity;
    }

    public void diveFalse(){
        m_Rigidbody2D.gravityScale = original_gravity;
    }

    private void FixedUpdate(){
        // m_Rigidbody2D.AddForce(new Vector2(min_xforce, min_yforce));
        // m_Rigidbody2D.velocity = transform.right * min_xforce;
        // if (m_Rigidbody2D.velocity[1] < min_xforce){
        //   m_Rigidbody2D.velocity = new Vector2(min_xforce, min_yforce);
        //}
        if(!grounded){
          m_Rigidbody2D.drag = airDrag;
        }
        if(grounded){
          m_Rigidbody2D.AddForce(new Vector2(min_xforce , 0));
            if (uphill)
            {
                m_Rigidbody2D.AddForce(new Vector2(0, Math.Max(min_xforce, min_xforce * slopeNormalPerpendicular.y * -1f * 10000f)));
                Debug.Log("Going Up");
            }
          // m_Rigidbody2D.drag = 0;
        }
        /*if(grounded && uphill){
            // m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x + min_xforce * slopeNormalPerpendicular.x * -1f, m_Rigidbody2D.velocity.y+ min_xforce*slopeNormalPerpendicular.y * -1f);
            //m_Rigidbody2D.AddForce(new Vector2(0, Math.Max(min_xforce, min_xforce * slopeNormalPerpendicular.y * -1f)));
            m_Rigidbody2D.AddForce(new Vector2(0, Math.Max(min_xforce, min_xforce * slopeNormalPerpendicular.y * -1f * 100f)));
            Debug.Log("Going Up");
          // Debug.Log(m_Rigidbody2D.velocity);
        }*/
        // if(grounded && !uphill && !downhill){
        //   m_Rigidbody2D.AddForce(new Vector2(min_xforce, 0));
        // }
        

        
    }
    
    //SpawnPosition transforms the jank location to a little bit above the spline point number #150; since 150 is even, it should always start at the tip of the hill
    //Should call this function every time respawn;
    public void SpawnPosition(){
        splineReference = TerrainGen.shape.spline.GetPosition(150)*TerrainGen.transform.localScale.x;
        transform.position = new Vector3(splineReference.x, splineReference.y + colliderSize.y + 0.2f ,0);
        // transform.position = new Vector3(0,60,0);
        // Debug.Log(transform.position);
    }
    void OnCollisionEnter2D(Collision2D col){
      
        grounded = true;
        //Check for slope, if on slope, need to add a vertical velocity perpendicular to slope to prevent it from falling down
    }

    void OnCollisionExit2D(Collision2D col){
        grounded = false;
    }

    private void slopeDeclare(float slopeSideAngle){
      if(slopeNormalPerpendicular.y <= 0){
        uphill = true;
        downhill = false;
      } else if (slopeNormalPerpendicular.y > 0){
        uphill = false;
        downhill = true;
      }
    }
    private void SlopeCheck() {
      Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, colliderSize.y));
      SlopeCheckHorizontal(checkPos);
      SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos) {
      RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance);
      RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, - transform.right, 0.5f);

      if (slopeHitFront) {
        isOnSlope = true;
        slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        Debug.DrawRay(slopeHitFront.point, slopeHitFront.normal, Color.green);
        slopeDeclare(slopeSideAngle);

      }

      else if (slopeHitBack) {
        isOnSlope = true;
        slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        Debug.DrawRay(slopeHitBack.point, slopeHitBack.normal, Color.blue);
        slopeDeclare(slopeSideAngle);
      }

      else {
        isOnSlope = false;
        slopeSideAngle = 0.0f;
      }
    }

    private void SlopeCheckVertical(Vector2 checkPos) {
      RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance);

      if (hit) {
        slopeNormalPerpendicular = Vector2.Perpendicular(hit.normal).normalized; //points to left of ground
        slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up); //angle between y-axis and normal (same as angle between x-axis and slope)

        if (slopeDownAngle != 0.0f) {
          isOnSlope = true;
        }

        Debug.DrawRay(hit.point, hit.normal, Color.white);
        Debug.DrawRay(hit.point, slopeNormalPerpendicular, Color.red);

      }
    }

    public IEnumerator Reset(){
      TerrainGen.generateTerrain();
      yield return new WaitUntil(isReady);
      SpawnPosition();
      m_Rigidbody2D.velocity = Vector3.zero;
    }

    // public void Reset(){
    //   TerrainGen.generateTerrain();
    //   SpawnPosition();
    //   m_Rigidbody2D.velocity = Vector3.zero;
    // }
    

    bool isReady(){
      if(TerrainGen.shape.spline.GetPosition(150).y > 1){
        return true;
      }
      else{
        return false;
      }
    }


}
