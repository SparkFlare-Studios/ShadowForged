using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Horizontal Movement Settings")]
    private Rigidbody2D rb;
    [SerializeField] private float WalkSpeed = 1;
    private float xAxis;


    [Header("Ground Check Settings")]
    private float jumpForce = 45 ;
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckY = 0.2f;
     [SerializeField] private float groundCheckX = 0.5f;
    [SerializeField] private LayerMask WhatIsGround;


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
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInputs();
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
        }
        if(Input.GetButtonDown("Jump")&& Grounded()){
            rb.velocity = new Vector3(rb.velocity.x, jumpForce);
        }
    }
}
