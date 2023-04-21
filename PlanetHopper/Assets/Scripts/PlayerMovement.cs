using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    public float crouchSpeed;
    private float moveSpeed;
    public float jumpForce;
    public float groundDrag;
    
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    // public Transform orientation;
    float horizontalInput;
    float verticalInput;
    bool jumpInput;
    float lastJumpTime = 0f;
    bool isCrouching = false;
    public float crouchScale;

    Vector3 moveDirection;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // rb.freezeRotation = true;
    }

    private void MyInput(){
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        jumpInput = Input.GetButton("Jump");
        if(!isCrouching){
            if(Input.GetButton("Run")) moveSpeed = sprintSpeed;
            else moveSpeed = walkSpeed;
        }
        if(Input.GetButtonDown("Crouch") && !isCrouching){
            isCrouching = true;
            moveSpeed=crouchSpeed;
            //TODO crouch
            transform.localScale = new Vector3(transform.localScale.x, crouchScale, transform.localScale.z);
            rb.AddForce(-transform.up*5f,ForceMode.Impulse);
        }
        if(Input.GetButtonUp("Crouch") && isCrouching){
            isCrouching = false;
            moveSpeed = walkSpeed;
            //TODO undo crouch
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);

        }
    }



    private void ClearInput(){
        horizontalInput = 0f;
        verticalInput = 0f;
        jumpInput = false;
    }
    // Update is called once per frame
    void Update()
    {
        //ground check
        grounded = Physics.Raycast(transform.position, - transform.up, playerHeight*0.5f+0.1f,whatIsGround);
        if (grounded){
            MyInput();
        }
        else{
            ClearInput();
        }
    }

    private void MovePlayer(){
        //planar motion
        moveDirection = transform.forward* verticalInput + transform.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // jump
        if (jumpInput && (Time.time-lastJumpTime >0.1f)){
            rb.AddForce(transform.up.normalized*jumpForce,ForceMode.Impulse);
            lastJumpTime=Time.time;
        }
    }

    private void FixedUpdate() {
        MovePlayer();
        SpeedLimit();
    }

    private void SpeedLimit(){
        Vector3 forwardVel = Vector3.Dot(rb.velocity,transform.forward)*transform.forward+Vector3.Dot(rb.velocity,transform.right)*transform.right;
        if(forwardVel.magnitude > moveSpeed){
            Vector3 maxVel = forwardVel.normalized*moveSpeed + transform.up*Vector3.Dot(rb.velocity,transform.up);
            rb.velocity = maxVel;
        }
        // Debug.Log(forwardVel.magnitude.ToString("n2") + " : " + rb.velocity.magnitude.ToString("n2"));

    }
}
