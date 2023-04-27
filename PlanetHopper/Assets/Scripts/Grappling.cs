using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grappling : MonoBehaviour
{
    public Transform cam;
    public Transform gunTip;
    public LayerMask whatIsGrappable;
    public LineRenderer lr;

    public float maxDist;
    public float grappleDelayTime;
    
    private Vector3 grapplePoint;
    private bool isGrappling = false;
    private RaycastHit hit;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire2")){
            beginGrapple();
        }
    }

    private void LateUpdate() {
        if(isGrappling) lr.SetPosition(0,gunTip.position);
    }

    void beginGrapple(){
        isGrappling = true;
        if(Physics.Raycast(cam.position,cam.forward,out hit, maxDist,whatIsGrappable)){
            grapplePoint=hit.point;
            Invoke(nameof(pullGrapple),grappleDelayTime);
        }else{
            grapplePoint = cam.position+cam.forward*maxDist;
            Invoke(nameof(cancelGrapple),grappleDelayTime);
        }
        lr.enabled = true;
        lr.SetPosition(1, grapplePoint);
    }

    void cancelGrapple(){
        isGrappling = false;
        lr.enabled = false;
    }

    void pullGrapple(){

    }
}