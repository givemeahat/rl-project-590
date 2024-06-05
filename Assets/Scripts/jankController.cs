using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;

public class jankController : MonoBehaviour
{
    public enum PlayerStatus { INLEVEL, FLYINGTONEXT };

    public PlayerStatus currentStatus;

    private Rigidbody2D m_Rigidbody2D;
    private jankGent jGent;

    [SerializeField] private float added_gravity;
    [SerializeField] private float original_gravity;
    [SerializeField] private float penalty_gravity;
    [SerializeField] private float airDrag;

    public bool grounded;
    public bool isDiving;

    private float slopeDownAngle;
    private Vector2 slopeNormalPerpendicular;
    private float slopeSideAngle;
    //private bool isOnSlope;
    private Vector2 colliderSize;
    public Transform checkPoint;

    public float storeYVel;

    public bool downhill;
    public bool uphill;

    public float min_xforce;

    [SerializeField] private bool reset = false;

    [SerializeField] private float slopeCheckDistance;

    [SerializeField] public terrainGen TerrainGen;
    private Vector3 splineReference;

    public int currentBonus = 10;
    public float multiplier = 1;

    public GameObject nyoomText;
    public Foliage.Foliage2D_Path fPath;

    public Vector2 _botRight;
    public LayerMask groundMask;
    public GameObject playerSprite;
    public Animator anim;
    public Rigidbody2D spriteRB;

    public GM gm;

    private bool nextLevelBoost = false;
    public bool notOnStart = false;

    public float speedPerc;

    private void Awake(){
        groundMask = LayerMask.GetMask("Ground");
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GM>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
		    colliderSize = GetComponents<Collider2D>()[0].bounds.extents;
        jGent = GetComponent<jankGent>();
    }
    private void Start()
    {
        // SpawnPosition();
    }

    // Update is called once per frame
    private void Update()
    {
        if (currentStatus == jankController.PlayerStatus.INLEVEL && gm.levelGenTriggered) gm.levelGenTriggered = false;
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
        if (m_Rigidbody2D.velocity.y <= 0)
        {
            Debug.Log("Downhill");
            if (uphill)
            {
                uphill = false;
                AdjustRotation();
            }
            downhill = true;
        }
        if (m_Rigidbody2D.velocity.y > 0 && storeYVel <= 0)
        {
            Debug.Log("Uphill");
            if (downhill)
            {
                downhill = false;
                if (grounded && isDiving && m_Rigidbody2D.velocity.x >= min_xforce + 1f) SpeedBoost();
            }
            uphill = true;
        }
        storeYVel = m_Rigidbody2D.velocity.y;
        //SlopeCheck();
        float _scanDistance = 5f;
        if (isDiving)
        {
            _scanDistance = 5000f;
        }

        /*if (grounded)
        {
            m_Rigidbody2D.AddForce(new Vector2(min_xforce, 0));
            if (uphill)
            {
                //m_Rigidbody2D.AddForce(new Vector2(0, Math.Max(min_xforce, min_xforce * -1f * 10000f)));
                m_Rigidbody2D.AddForce(new Vector2(0, Math.Max(min_xforce, min_xforce * slopeNormalPerpendicular.y * -1f)));
                Debug.Log("Going Up");
            }
            //m_Rigidbody2D.drag = 0;
        }*/

        /*if (hit)
        {
            if (hit.collider.gameObject.tag == "Ground")
            {
                currentStatus = PlayerStatus.INLEVEL;
                float computedAngle = Vector2.Angle(Vector2.up, hit.normal);
                if (downhill || downhill && isDiving)
                {
                    playerSprite.transform.rotation = Quaternion.Lerp(playerSprite.transform.rotation, Quaternion.Euler(0, 0, -computedAngle), 0.5f);
                }
                else if (uphill && grounded)
                {
                    playerSprite.transform.rotation = Quaternion.Lerp(playerSprite.transform.rotation, Quaternion.Euler(0, 0, computedAngle), 0.5f);

                    //playerSprite.transform.rotation = Quaternion.Euler(0, 0, computedAngle);
                }
            }
            else currentStatus = PlayerStatus.FLYINGTONEXT;
        }
        else { currentStatus = PlayerStatus.FLYINGTONEXT; }*/
    }

    public void LateUpdate()
    {
        float _scanDistance = 5000f;

        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.down, _scanDistance, groundMask);

        if (hit)
        {
            if (hit.collider.gameObject.tag == "Ground")
            {
                currentStatus = PlayerStatus.INLEVEL;
                nextLevelBoost = false;
                float computedAngle = Vector2.Angle(Vector2.up, hit.normal);
                if (downhill || downhill && isDiving)
                {
                    playerSprite.transform.rotation = Quaternion.Lerp(playerSprite.transform.rotation, Quaternion.Euler(0, 0, -computedAngle), 0.5f);
                }
                else if (uphill && grounded)
                {
                    playerSprite.transform.rotation = Quaternion.Lerp(playerSprite.transform.rotation, Quaternion.Euler(0, 0, computedAngle), 0.5f);
                    //playerSprite.transform.rotation = Quaternion.Euler(0, 0, computedAngle);
                }
            }
            else currentStatus = PlayerStatus.FLYINGTONEXT;
        }
        else { currentStatus = PlayerStatus.FLYINGTONEXT;
            if (!nextLevelBoost)
            {
                m_Rigidbody2D.AddForce(new Vector2(1000f, 2000f));
                nextLevelBoost = true;
            }
        }
    }

    public void AdjustRotation()
    {
        float _scanDistance = 5000f;
        RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector2.down, _scanDistance, groundMask);

        if (hit)
        {
            if (hit.collider.gameObject.tag == "Ground")
            {
                float computedAngle = Vector2.Angle(Vector2.up, hit.normal);
                StartCoroutine(SmoothRotate(computedAngle));
            }
        }
    }
    IEnumerator SmoothRotate(float computedAngle)
    {
        float _f = 0f;
        float endTime = 5f*m_Rigidbody2D.velocity.x;
        while (_f < endTime)
        {
            if (isDiving || uphill) yield break;
            _f += Time.deltaTime;
            playerSprite.transform.rotation = Quaternion.Lerp(playerSprite.transform.rotation, Quaternion.Euler(0, 0, -computedAngle).normalized, _f/endTime);
            yield return null;
        }
        playerSprite.transform.rotation = Quaternion.Euler(0, 0, -computedAngle).normalized;
    }

    public void SpeedBoost()
    {
        Debug.Log("NYOOOOM");
        jGent.reward += 1000f;
        gm.ActivateBonusScoreText();
        nyoomText.SetActive(false);
        nyoomText.SetActive(true);
        m_Rigidbody2D.AddForce(new Vector2(m_Rigidbody2D.velocity.y + (currentBonus * multiplier), m_Rigidbody2D.velocity.x + (currentBonus * multiplier)));
        if (multiplier < 3f) multiplier = multiplier + .5f;
        else multiplier = 3f;
    }
    public void DisableNyoomTxt()
    {
        nyoomText.SetActive(false);
    }
    public void dive(){
        m_Rigidbody2D.gravityScale = added_gravity;
        isDiving = true;
    }

    public void diveFalse(){
        m_Rigidbody2D.gravityScale = original_gravity;
        isDiving = false;
    }

    private void FixedUpdate(){
        //causes weird choppiness when flying
        /*if(!grounded){
          m_Rigidbody2D.drag = airDrag;
        }*/
        if (m_Rigidbody2D.velocity.x > 150)
        {
            m_Rigidbody2D.velocity = new Vector2(150f, m_Rigidbody2D.velocity.y);
            Debug.Log("Velocity: " + m_Rigidbody2D.velocity.x);
        }
        speedPerc = (m_Rigidbody2D.velocity.x / 150f);
        anim.SetFloat("Speed", speedPerc);
        if (grounded){
          m_Rigidbody2D.AddForce(new Vector2(min_xforce, 0));
            if (uphill)
            {
                //m_Rigidbody2D.AddForce(new Vector2(0, Math.Max(min_xforce, min_xforce * -1f * 10000f)));
                m_Rigidbody2D.AddForce(new Vector2(0, Math.Max(min_xforce, min_xforce * slopeNormalPerpendicular.y * -1f * 1000f)));
                Debug.Log("Going Up");
            }
            //m_Rigidbody2D.drag = 0;
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
        //splineReference = TerrainGen.shape.spline.GetPosition(3)*TerrainGen.transform.localScale.x;
        //transform.position = new Vector3(splineReference.x, splineReference.y + colliderSize.y + 0.2f ,0);
        // Debug.Log(transform.position);
    }
    void OnCollisionEnter2D(Collision2D col){

        Debug.Log("grounded");
        grounded = true;
        anim.SetBool("Grounded", true);
            //Check for slope, if on slope, need to add a vertical velocity perpendicular to slope to prevent it from falling down
        }

    void OnCollisionExit2D(Collision2D col){
        Debug.Log("Not grounded");
        grounded = false;
        anim.SetBool("Grounded", false);
    }

    /*private void slopeDeclare(float slopeSideAngle){
      if(slopeNormalPerpendicular.y <= 0){
        uphill = true;
        downhill = false;
      } else if (slopeNormalPerpendicular.y > 0){
        uphill = false;
        downhill = true;
      }
    }
    private void SlopeCheck() {
        Vector2 checkPos = new Vector2 (checkPoint.position.x, checkPoint.position.y);
        //Vector2 checkPos = transform.position - (Vector3)(new Vector2(0f, colliderSize.y));
        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos) {
        /*RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, 0.5f);

        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, 0.5f);

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
    }*/

    public IEnumerator Reset(){
      TerrainGen.generateTerrain();
      yield return new WaitUntil(isReady);
        currentStatus = PlayerStatus.INLEVEL;
        SpawnPosition();
      m_Rigidbody2D.velocity = Vector3.zero;
    }

    bool isReady(){
      if(TerrainGen.isDone){
            notOnStart = true;
            return true;
      }
      else{
        return false;
      }
    }


}
