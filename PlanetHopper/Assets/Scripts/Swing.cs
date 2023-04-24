using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swing : MonoBehaviour
{
    private bool isSwinging = false;
    public LineRenderer line;
    public Transform gunTip, cam, player;
    public LayerMask whatIsGrappleable;

    public float maxLineDist;
    private Vector3 swingPoint, currentGrapplePosition;
    private SpringJoint joint;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire2")){
            isSwinging = !isSwinging;
            if(isSwinging) StartSwing();
            else StopSwing();
        }
        // if(Input.GetButtonDown("Jump")){
        //     if (isSwinging){
        //         StopSwing();
        //         isSwinging=false;
        //     }
        // }
    }

    void LateUpdate(){
        DrawRope();
    }

    void StartSwing(){
        RaycastHit hit;
        if(Physics.Raycast(cam.position, cam.forward, out hit, maxLineDist, whatIsGrappleable)){
            swingPoint = hit.point;
            joint = player.gameObject.AddComponent<SpringJoint>();
            joint.autoConfigureConnectedAnchor = false;
            joint.connectedAnchor = swingPoint;

            float dist = Vector3.Distance(player.position,swingPoint);
            joint.maxDistance = dist*0.8f;
            joint.minDistance = 0f;

            joint.spring = 1f;
            joint.damper = 7f;
            joint.massScale = 4.5f;

            line.positionCount = 2;
            currentGrapplePosition = gunTip.position;
        }
    }

    void StopSwing(){
        line.positionCount = 0;
        Destroy(joint);
    }

    void DrawRope(){
        if(!joint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, swingPoint, Time.deltaTime*8f);
        line.SetPosition(0, gunTip.position);
        line.SetPosition(1, currentGrapplePosition);
    }
}
