using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class jankController : MonoBehaviour
{
    private Rigidbody2D m_Rigidbody2D;

    [SerializeField] private float addedGravity;
    [SerializeField] private float originalGravity;

    [SerializeField] private float addedAirDrag;

    private bool grounded;
    private ContactPoint2D contact;
    private Vector2 addedForce;

    private float slopeDownAngle;
    private Vector2 slopeDown;
    // private float slopeSideAngle;
    private Vector2 contactSlope;
    internal float contactSlopeAngle;
    private float slope;

    // private bool isOnSlope;
    private Vector2 colliderSize;
    public float timeSinceHop;
    public float timeSinceJet;
    public float hopCoolDown;
    public float jetCoolDown;


    private bool downhill;
    private bool uphill;

    public float min_xforce;

    [SerializeField] private bool reset = false;

    // [SerializeField] private float slopeCheckDistance;

    [SerializeField] private terrainGen TerrainGen;
    private Vector3 splineReference; 



    private void Awake(){
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
		    colliderSize = GetComponents<Collider2D>()[0].bounds.extents;

    }
    private void Start()
    {
      // SpawnPosition();
      timeSinceHop = hopCoolDown;
      timeSinceJet = jetCoolDown;
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
        // if(Input.GetButtonDown("Dive")){
        //   dive();
        // }
        // if(Input.GetButtonUp("Dive")){
        //   diveFalse();
        // }

        // SlopeCheck();

        if(timeSinceHop < hopCoolDown){
          timeSinceHop += Time.deltaTime;
        }
        if(timeSinceJet < jetCoolDown){
          timeSinceJet += Time.deltaTime;
        }
    }

    public void dive(){
        // if(uphill && grounded){
        //   return;
        // }
        m_Rigidbody2D.gravityScale = addedGravity;
    }

    public void diveFalse(){
        m_Rigidbody2D.gravityScale = originalGravity;
    }

    public void jetPack(){
      if(timeSinceJet < jetCoolDown){
        return;
      }
      addedForce = new Vector2(300f, 0f);
      // addedForce = new Vector2(min_xforce * contactSlope.x * -1f, min_xforce * contactSlope.y * -1f);

      m_Rigidbody2D.AddForce(addedForce);
      timeSinceJet = 0f; //reset timer
      Debug.Log("jet");
    }
    public void hop(){
      if(timeSinceHop < hopCoolDown){
        return;
      }
      addedForce = new Vector2(0f, 300f);
      // addedForce = new Vector2(min_xforce * contactSlope.x * -1f, min_xforce * contactSlope.y * -1f);

      m_Rigidbody2D.AddForce(addedForce);
      timeSinceHop = 0f; //reset timer
      Debug.Log("hop");
    }

    private void FixedUpdate(){
        // m_Rigidbody2D.AddForce(new Vector2(min_xforce, min_yforce));
        // m_Rigidbody2D.velocity = transform.right * min_xforce;
        // if (m_Rigidbody2D.velocity[1] < min_xforce){
        //   m_Rigidbody2D.velocity = new Vector2(min_xforce, min_yforce);
        //}
        if(!grounded){
          SlopeCheckDown();
          m_Rigidbody2D.drag = addedAirDrag;
        }
        if(grounded){
          m_Rigidbody2D.drag = 0f;

          // m_Rigidbody2D.AddForce(new Vector2(min_xforce , 0));
          Debug.DrawRay(transform.position, contactSlope, Color.green);
          // m_Rigidbody2D.drag = 0;
        }
        slopeDeclare();
        if(grounded && uphill){
          // m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x + min_xforce * slopeNormalPerpendicular.x * -1f, m_Rigidbody2D.velocity.y+ min_xforce*slopeNormalPerpendicular.y * -1f);
          
          // m_Rigidbody2D.AddForce(new Vector2(0, Math.Max(min_xforce, min_xforce * contactSlope.y * -1f)));
          // Debug.Log("should do something");
          // Debug.Log(m_Rigidbody2D.velocity);

          addedForce = new Vector2(min_xforce * contactSlope.x * -1f, 9.8f);
          // addedForce = new Vector2(min_xforce * contactSlope.x * -1f, min_xforce * contactSlope.y * -1f);

          m_Rigidbody2D.AddForce(addedForce);
        }
        if(grounded && downhill){
          addedForce = new Vector2(min_xforce * contactSlope.x * -1f , 0);
          m_Rigidbody2D.AddForce(addedForce);
        }
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
        // contact = col.GetContact(0);
        // contactSlope = Vector2.Perpendicular(contact.normal).normalized;
        // contactSlopeAngle = 
        //Check for slope, if on slope, need to add a vertical velocity perpendicular to slope to prevent it from falling down
    }
      void OnCollisionStay2D(Collision2D col)
    {
        slope = ContactSlopeAngle(col);
        //Check for slope, if on slope, need to add a vertical velocity perpendicular to slope to prevent it from falling down
    }

    private float ContactSlopeAngle(Collision2D col)
    {
        contact = col.GetContact(0);
        contactSlope = Vector2.Perpendicular(contact.normal).normalized;
        contactSlopeAngle = Vector2.Angle(Vector2.up, contactSlope);
        return contactSlopeAngle;
    }

    void OnCollisionExit2D(Collision2D col){
        grounded = false;
    }

    private void slopeDeclare(){
      if(slope > 180f){
        Debug.Log("something's wrong with the slope");
      }
      if(slope > 90f){
        uphill = true;
        downhill = false;
      } else if (slope < 90f){
        uphill = false;
        downhill = true;
      }
    }
    // private void SlopeCheck() {
    //   // Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, colliderSize.y)); //point at ground level
    //   Vector2 checkPos = transform.position;
    //   // SlopeCheckHorizontal(checkPos);
    //   SlopeCheckDown();
    // }

    // private void SlopeCheckHorizontal(Vector2 checkPos) {
    //   // RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance);
    //   RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, Vector2.right, colliderSize.y);
    //   RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, Vector2.left , colliderSize.y);

    //   if (slopeHitFront) {
    //     // isOnSlope = true;
    //     slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
    //     Debug.DrawRay(slopeHitFront.point, slopeHitFront.normal, Color.green);
    //     slopeDeclare(slopeSideAngle);

    //   }

    //   else if (slopeHitBack) {
    //     // isOnSlope = true;
    //     slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
    //     Debug.DrawRay(slopeHitBack.point, slopeHitBack.normal, Color.blue);
    //     slopeDeclare(slopeSideAngle);
    //   }

    //   else {
    //     // isOnSlope = false;
    //     slopeSideAngle = 0.0f;
    //   }
    // }

    private void SlopeCheckDown() {
      Vector2 checkPos = transform.position - new Vector3(0.0f, colliderSize.y + 0.1f,0.0f);
      RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, Mathf.Infinity);

      if (hit) {
        slopeDown = Vector2.Perpendicular(hit.normal).normalized; //vector normal to the hit normal, this is the slope when raycasting down
        slopeDownAngle = Vector2.Angle(slopeDown, Vector2.up); //angle between y-axis and normal (same as angle between x-axis and slope)
        slope = slopeDownAngle;
        // if (slopeDownAngle != 0.0f) {
        //   isOnSlope = true;
        // }

        // Debug.DrawRay(hit.point, hit.normal, Color.white);
        // Debug.DrawRay(checkPos, Vector2.down, Color.white);

      }
    }
    void OnDrawGizmosSelected()
    {
      Gizmos.color = Color.red;
      Gizmos.DrawRay(transform.position, slopeDown);
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
