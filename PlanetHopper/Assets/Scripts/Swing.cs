using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    private bool isSwinging = false, isGrappling = false;
    public LineRenderer line;
    public Transform gunTip, cam, predictionPoint;
    private PlayerMovement playerMovement;
    public LayerMask whatIsGrappleable;

    public float maxLineDist, grappleDelayTime, ropeSpeed, sphereCastRadius, grappleOvershoot;
    float currentLineDist, grappleTime, grappleTimeOut;
    private Vector3 swingPoint, currentGrapplePosition;
    private SpringJoint joint;
    RaycastHit hitRay, hitSphere;
    GameObject hitObject;
    Rigidbody rb;
    PlayerGravity gravity;

    [Header("Sound Effects")]
    public FMODUnity.EventReference grappleSFX;
    public FMODUnity.EventReference grapplePullSFX;
    public FMODUnity.EventReference grappleReleaseSFX;
    public FMODUnity.EventReference grappleHitSFX;
    
    private Platform platform = null;

    // Start is called before the first frame update
    void Start()
    {
        platform = null;
        playerMovement = GetComponent<PlayerMovement>();
        rb = GetComponent<Rigidbody>();
        gravity = GetComponent<PlayerGravity>();

    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.IsAcceptingPlayerInput() && Input.GetButtonDown("Fire2")){
            isSwinging = !isSwinging;
            if(isSwinging) StartSwing();
            else StopGrapple();
        }
        if(joint){
            if(GameManager.instance.IsAcceptingPlayerInput() && Input.GetKey(KeyCode.E)){
                currentLineDist-=ropeSpeed;
                joint.maxDistance = currentLineDist;
                rb.AddForce(swingPoint-transform.position,ForceMode.Force);
            }
            else if(GameManager.instance.IsAcceptingPlayerInput() && Input.GetKey(KeyCode.Q)){
                currentLineDist+=ropeSpeed;
                joint.maxDistance = currentLineDist;
            }
            if(GameManager.instance.IsAcceptingPlayerInput() && Input.GetButtonDown("Crouch")){
                PullGrapple();
            }
        
        }
        if(!isSwinging){
            CheckSwingPoint();
        }else if(GameManager.instance.IsAcceptingPlayerInput() && Input.GetButtonDown("Jump")){
            StopGrapple();
        }
    }

    void LateUpdate(){
        DrawRope();
        if(isGrappling){
            rb.drag=0;
            if(Vector3.Distance(swingPoint,transform.position)<1f || Time.time-grappleTime>grappleTimeOut){
                StopGrapple();
            }
        }
    }

    void CheckSwingPoint(){
        if(Physics.Raycast(cam.position, cam.forward, out hitRay, maxLineDist, whatIsGrappleable)){
            swingPoint = hitRay.point;
            if(hitRay.collider.CompareTag("Platform"))
            {
                if (platform)
                {
                    platform.unsetGrapple();
                }
                platform = hitRay.collider.GetComponent<Platform>();
                platform.setGrapple(this);
            }
            predictionPoint.position = swingPoint;
            predictionPoint.localScale = Vector3.one*hitRay.distance*0.02f;

            hitObject = hitRay.collider.gameObject;

        }else if(Physics.SphereCast(cam.position, sphereCastRadius, cam.forward, out hitSphere, maxLineDist, whatIsGrappleable)){    
            swingPoint = hitSphere.point;
            if(hitSphere.collider.CompareTag("Platform"))
            {
                if (platform)
                {
                    platform.unsetGrapple();
                }
                platform = hitSphere.collider.GetComponent<Platform>();
                platform.setGrapple(this);
            }
            predictionPoint.position = swingPoint;
            predictionPoint.localScale = Vector3.one*hitSphere.distance*0.02f;

            hitObject = hitSphere.collider.gameObject;
        }else{
            swingPoint = Vector3.zero;
            predictionPoint.localScale = Vector3.zero;
        }
    }
    void StartSwing(){
        FMODUnity.RuntimeManager.PlayOneShot(grappleSFX, transform.position);

        if(swingPoint != Vector3.zero){    

            //Verifies if the final object was the finish objective
            if(hitObject.CompareTag("Finish")){
                GameManager.instance.Win();
            }
            
            joint = transform.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            currentLineDist = Vector3.Distance(transform.position,swingPoint);
            joint.maxDistance = currentLineDist;
            joint.minDistance = 0f;

            joint.spring = 4f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            line.positionCount = 2;
            currentGrapplePosition = gunTip.position;
            playerMovement.airDashAvailable=true;

            FMODUnity.RuntimeManager.PlayOneShot(grappleHitSFX, transform.position);

        }else{
            currentGrapplePosition = gunTip.position;
            line.positionCount = 2;
            swingPoint=cam.position+cam.forward*maxLineDist;
            Invoke(nameof(StopSwing),grappleDelayTime);
        }


    }

    void StopSwing(){
        line.positionCount = 0;
        isSwinging = false;
        Destroy(joint);
    }

    void PullGrapple(){
        isGrappling = true;
        Destroy(joint);
        grappleTime = Time.time;
        rb.velocity = CalculateLaunchVelocity();

        FMODUnity.RuntimeManager.PlayOneShot(grapplePullSFX, transform.position);
    }

    public void StopGrapple(){
        if (platform)
        {
            platform.unsetGrapple();
            platform = null;
        }
        isGrappling = false;
        StopSwing();

        FMODUnity.RuntimeManager.PlayOneShot(grappleReleaseSFX, transform.position);


        // joint = player.gameObject.AddComponent<SpringJoint>();
        // joint.autoConfigureConnectedAnchor = false;
        // joint.connectedAnchor = swingPoint;

        // joint.maxDistance = currentLineDist;
        // joint.minDistance = 0f;

        // joint.spring = 4f;
        // joint.damper = 7f;
        // joint.massScale = 4.5f;
    }
    
    public Vector3 getSwingPoint()
    {
        return swingPoint;
    }
    public void setSwingPoint(Vector3 newPoint)
    {
        currentGrapplePosition = newPoint;
        swingPoint = newPoint;
        if(joint) joint.connectedAnchor = swingPoint;
        predictionPoint.localScale = Vector3.zero;
    }

    Vector3 CalculateLaunchVelocity(){
        float g = gravity.GetGravity();
        if (gravity.IsInSpace()){
            Vector3 dir = swingPoint-transform.position;
            return dir;
        }
        Vector3 displacement = swingPoint-transform.position;
        float verticalDisplacement = Vector3.Dot(displacement, transform.up);
        float h = grappleOvershoot + verticalDisplacement;
        if(verticalDisplacement<0) h = grappleOvershoot;
        Vector3 lateralDisplacement = displacement-verticalDisplacement*transform.up;

        Vector3 verticalVelocity = transform.up * Mathf.Sqrt(2*g*h);
        grappleTimeOut = Mathf.Sqrt(2*h/g)+Mathf.Sqrt(-2*(verticalDisplacement-h)/g);
        Vector3 lateralVelocity = lateralDisplacement / grappleTimeOut;
        return verticalVelocity+lateralVelocity;
    }

    void DrawRope(){
        if(!isSwinging) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime*8f);
        line.SetPosition(0, gunTip.position);
        line.SetPosition(1, currentGrapplePosition);
    }

    
}
