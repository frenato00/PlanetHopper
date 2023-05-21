using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    private bool isSwinging = false, isGrappling = false;
    public LineRenderer line;
    public Transform gunTip, cam, player, predictionPoint;
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

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();
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
                rb.AddForce(swingPoint-player.position,ForceMode.Force);
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
            predictionPoint.position = swingPoint;
            predictionPoint.localScale = Vector3.one*hitRay.distance*0.02f;

            hitObject = hitRay.collider.gameObject;

        }else if(Physics.SphereCast(cam.position, sphereCastRadius, cam.forward, out hitSphere, maxLineDist, whatIsGrappleable)){    
            swingPoint = hitSphere.point;
            predictionPoint.position = swingPoint;
            predictionPoint.localScale = Vector3.one*hitSphere.distance*0.02f;

            hitObject = hitSphere.collider.gameObject;
        }else{
            swingPoint = Vector3.zero;
            predictionPoint.localScale = Vector3.zero;
        }
    }
    void StartSwing(){
        if(swingPoint != Vector3.zero){    

            if(hitObject.CompareTag("Finish")){
                GameManager.instance.Win();
            }
            
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            currentLineDist = Vector3.Distance(player.position,swingPoint);
            joint.maxDistance = currentLineDist;
            joint.minDistance = 0f;

            joint.spring = 4f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            line.positionCount = 2;
            currentGrapplePosition = gunTip.position;
            playerMovement.airDashAvailable=true;
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
    }

    void StopGrapple(){
        isGrappling = false;
        StopSwing();
        // joint = player.gameObject.AddComponent<SpringJoint>();
        // joint.autoConfigureConnectedAnchor = false;
        // joint.connectedAnchor = swingPoint;

        // joint.maxDistance = currentLineDist;
        // joint.minDistance = 0f;

        // joint.spring = 4f;
        // joint.damper = 7f;
        // joint.massScale = 4.5f;
    }

    Vector3 CalculateLaunchVelocity(){
        float g = gravity.GetGravity();
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
