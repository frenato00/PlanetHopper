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

    public float dashImpulse;
    public float shortDashCooldown;
    public float dashCooldown;
    public int maxDashes;
    float dashTime = 0f;
    int dashCounter = 0;
    bool isDashing = false;
    
    [Header("Sound Effects")]
    public FMODUnity.EventReference walkSFX;
    public FMODUnity.EventReference runSFX;
    public FMODUnity.EventReference jumpSFX;
    public FMODUnity.EventReference dashSFX;
    public FMODUnity.EventReference landSFX;
    
    FMOD.Studio.EventInstance playerWalking;
    FMOD.Studio.EventInstance playerRunning;

    bool isWalkSFXPlaying = false;
    bool isRunSFXPlaying = false;

    bool wasGrounded = true;

    [HideInInspector]
    public bool airDashAvailable=true;
    float forwardMag;

    Vector3 moveDirection;

    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerWalking = FMODUnity.RuntimeManager.CreateInstance(walkSFX);
        playerRunning = FMODUnity.RuntimeManager.CreateInstance(runSFX);
        // rb.freezeRotation = true;
    }

    private void MyInput(){
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        jumpInput = Input.GetButton("Jump");
        if(grounded) airDashAvailable=true;
        if(!isCrouching){
            if(GameManager.instance.IsAcceptingPlayerInput() && Input.GetButton("Run")) {
                moveSpeed = sprintSpeed;
                if(grounded){
                    if(!isRunSFXPlaying){
                        playerRunning.start();
                    }
                    if(isWalkSFXPlaying){
                        playerWalking.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    }
                    
                }
            } 
            else{
                moveSpeed = walkSpeed;
                if(grounded){
                    if(!isWalkSFXPlaying){
                        playerWalking.start();
                    }
                    if(isRunSFXPlaying){
                        playerRunning.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    }
                }
            }

        }
        if(GameManager.instance.IsAcceptingPlayerInput() && Input.GetButtonDown("Crouch") && !isCrouching){
            isCrouching = true;
            moveSpeed=crouchSpeed;
            //TODO crouch
            transform.localScale = new Vector3(transform.localScale.x, crouchScale, transform.localScale.z);
            rb.AddForce(-transform.up*5f,ForceMode.Impulse);
        }
        if(GameManager.instance.IsAcceptingPlayerInput() && Input.GetButtonUp("Crouch") && isCrouching){
            isCrouching = false;
            moveSpeed = walkSpeed;
            //TODO undo crouch
            transform.localScale = new Vector3(transform.localScale.x, 1, transform.localScale.z);

        }
        if(GameManager.instance.IsAcceptingPlayerInput() && Input.GetButtonDown("Run")&& !isCrouching && grounded){
            if (dashCounter < maxDashes && Time.time - dashTime > shortDashCooldown){
                isDashing = true;
                dashTime = Time.time;
                dashCounter++;
            } else if(Time.time - dashTime > dashCooldown){
                isDashing = true;
                dashTime = Time.time;
                dashCounter = 1;
            }
        }
        if(GameManager.instance.IsAcceptingPlayerInput() && !grounded && airDashAvailable && Input.GetButtonDown("Jump")){
            isDashing=true;
            airDashAvailable=false;
        }


        //Blocks the playing if they are stopped or airborne
        if(horizontalInput == 0 && verticalInput== 0 || !grounded ){
            playerWalking.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            playerRunning.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }
    }



    // private void AirInput(){
    //     // horizontalInput = 0f;
    //     // verticalInput = 0f;
    //     jumpInput = false;
    // }
    // Update is called once per frame
    void Update()
    {
        //ground check
        grounded = Physics.Raycast(transform.position, - transform.up, playerHeight*0.5f+0.1f,whatIsGround);
        // if (grounded){
        MyInput();
        // }
        // else{
        //     AirInput();
        // }

        UpdateSFXStates();

        if(grounded && !wasGrounded){
            FMODUnity.RuntimeManager.PlayOneShot(landSFX, transform.position);
        }
        wasGrounded = grounded;  
    }

    private void UpdateSFXStates(){
        
        FMOD.Studio.PLAYBACK_STATE walkSFXState;
        FMOD.Studio.PLAYBACK_STATE runSFXState;

        playerWalking.getPlaybackState(out walkSFXState);
        playerRunning.getPlaybackState(out runSFXState);

        isRunSFXPlaying = CheckIfIsPlaying(runSFXState);
        isWalkSFXPlaying = CheckIfIsPlaying(walkSFXState);

    }

    private bool CheckIfIsPlaying(FMOD.Studio.PLAYBACK_STATE state){
        if(state == FMOD.Studio.PLAYBACK_STATE.PLAYING || state == FMOD.Studio.PLAYBACK_STATE.STARTING){
            return true;
        }

        return false;
    }



    private void MovePlayer(){
        //planar motion
        moveDirection = transform.forward* verticalInput + transform.right * horizontalInput;
        Vector3 forwardVel = Vector3.Dot(rb.velocity,transform.forward)*transform.forward+Vector3.Dot(rb.velocity,transform.right)*transform.right;
        if(forwardVel.magnitude > moveSpeed){
            //if it is going above speed limit it can only decelerate or change direction
            forwardMag = Vector3.Dot(forwardVel.normalized,moveDirection);
            if (forwardMag>0){
                // if force in the direction of movement add only the coponent normal to the movement
                moveDirection -= forwardVel.normalized*forwardMag;
            }
        }
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        // jump
        if (grounded && jumpInput && (Time.time-lastJumpTime >0.1f)){
            rb.AddForce(transform.up.normalized*jumpForce,ForceMode.Impulse);
            lastJumpTime=Time.time;

            FMODUnity.RuntimeManager.PlayOneShot(jumpSFX, transform.position);
        }
        // Debug.Log("Forward:"+forwardVel.magnitude.ToString("n2") + "; Total: " + rb.velocity.magnitude.ToString("n2"));
    
        if(isDashing){
            isDashing = false;
            if(!grounded) rb.AddForce(transform.up.normalized*jumpForce,ForceMode.Impulse);
            else rb.AddForce(moveDirection.normalized*dashImpulse,ForceMode.Impulse);

            FMODUnity.RuntimeManager.PlayOneShot(dashSFX, transform.position);
        }
    }

    private void FixedUpdate() {
        MovePlayer();
        // LimitSpeed();
    }

    //Deprecated
    private void LimitSpeed(){
        Vector3 forwardVel = Vector3.Dot(rb.velocity,transform.forward)*transform.forward+Vector3.Dot(rb.velocity,transform.right)*transform.right;
        if(forwardVel.magnitude > moveSpeed){
            Vector3 maxVel = forwardVel.normalized*moveSpeed + transform.up*Vector3.Dot(rb.velocity,transform.up);
            rb.velocity = maxVel;
        }
        Debug.Log("Forward:"+forwardVel.magnitude.ToString("n2") + "; Total: " + rb.velocity.magnitude.ToString("n2"));

    }
}
