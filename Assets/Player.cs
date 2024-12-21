using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [Header("Dash Info")]
    [SerializeField]private float dashDuration;
    [SerializeField]private float dashTime;

    private float xInput;
    private int facingDir = 1;
    private bool facingRight=true;

    [Header("Collision info")]
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask whatIsGround;
    private bool isGrounded;
    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        anim=GetComponentInChildren<Animator>();
    }

    void Update()
    {
        Movement();
        CheckInput();
        CollisionChecks();

        dashTime -=Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            dashTime=dashDuration;
        }

        if(dashTime > 0){
            Debug.Log("doing dash");
        }

        FlipController();
        AnimatorControllers();

    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
    }

    private void CheckInput()
    {
        xInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void Movement()
    {
        rb.velocity = new Vector2(xInput * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        if(isGrounded)
           rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }




    private void AnimatorControllers(){
        bool isMoving = rb.velocity.x!=0;

        anim.SetFloat("yVelocity", rb.velocity.y);
        anim.SetBool("isMoving",isMoving);
        anim.SetBool("isGrounded", isGrounded);
        
    }
    private void Flip(){
        facingDir = facingDir*-1;
        facingRight = !facingRight;
        transform.Rotate(0,180,0);
    }
    private void FlipController(){
        //如果行进方向与面朝方向不一致则转身。
        //这是为了后续的一些实现。
        if(rb.velocity.x>0 && !facingRight) 
            Flip();
        else if(rb.velocity.x<0 && facingRight)
            Flip();
    }

    private void OnDrawGizmos(){
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x,transform.position.y-groundCheckDistance));
    }
}
