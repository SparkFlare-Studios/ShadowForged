using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement Settings")]
    [SerializeField] private float WalkSpeed = 1;
    

    [Header("Vertical Movement Settings")]
    private float jumpForce = 45 ;
    private int JumpBufferCounter = 0;
    [SerializeField] private int JumpBufferFrames;
    private float coyoteTimeCounter = 0;
    [SerializeField] private float coyoteTime;
    private int airJumpCounter = 0;
    [SerializeField] private int maxAirJumps; 


    [Header("Ground Check Settings")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckY = 0.2f;
    [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask WhatIsGround;


    PlayerStateList pState;
    private Rigidbody2D rb;
    private float xAxis;

   
    public static PlayerController Instance;
    
    void Awake(){
        if (Instance != null && Instance != this){
            Destroy(gameObject);
        }else{
            Instance = this; 
        }
    }

    void Start()
    {
        pState = GetComponent<PlayerStateList>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
        UpdateJumpVariables();
        Move();
        Jump();
    }
    void GetInputs(){
        xAxis = Input.GetAxisRaw("Horizontal");
    }
    private void Move(){
        rb.velocity = new Vector2(WalkSpeed * xAxis, rb.velocity.y);
    }
    public bool Grounded(){
        if(Physics2D.Raycast(groundCheckPoint.position,Vector2.down,groundCheckY,WhatIsGround)
        || Physics2D.Raycast(groundCheckPoint.position + new Vector3(groundCheckX, 0 , 0) ,Vector2.down ,groundCheckY,WhatIsGround)
        || Physics2D.Raycast(groundCheckPoint.position + new Vector3(- groundCheckX, 0 , 0) ,Vector2.down ,groundCheckY,WhatIsGround)

        ){
            return true;
        }else{
            return false;
        }
    }
    void Jump(){
        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0){
               rb.velocity = new Vector2(rb.velocity.x, 0);
            pState.jumping = false;
        }
        if(!pState.jumping){
            if(JumpBufferCounter > 0 && coyoteTimeCounter > 0){
                rb.velocity = new Vector3(rb.velocity.x, jumpForce);
                pState.jumping = true;
            }
            else if(!Grounded() && airJumpCounter < maxAirJumps && Input.GetButtonDown("Jump")) {
                pState.jumping = true;
                airJumpCounter++; 
                 rb.velocity = new Vector3(rb.velocity.x, jumpForce);
            }
        }
       
    }
    void UpdateJumpVariables(){
        if(Grounded()){
            pState.jumping = false;
            coyoteTimeCounter = coyoteTime;
            airJumpCounter = 0;
        }else {
            coyoteTimeCounter -= Time.deltaTime;
        }
        if(Input.GetButtonDown("Jump")){
            JumpBufferCounter = JumpBufferFrames; 
        }
        else {
            JumpBufferCounter--;
        }
    }
}
