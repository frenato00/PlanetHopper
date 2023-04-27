using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGravity : MonoBehaviour
{

    Vector3 resultingForce;
    float gravity;
    int attractors=0;
    bool inSpace=true;
    Vector3 up;
    Rigidbody rb;
    public void AddForce(Vector3 gravity){
        resultingForce+=gravity;
        attractors++;
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        resultingForce = new Vector3(0f,0f,0f);
        up = new Vector3(0f,0f,0f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.AddForce(resultingForce, ForceMode.Force);
        if(resultingForce.magnitude > 0.1){
            up = -resultingForce.normalized;
            gravity = resultingForce.magnitude;
        }
        else{
            gravity = 0f;
        }
        resultingForce = new Vector3(0f,0f,0f);
        inSpace = attractors==0;
        attractors=0;
    }

    void Update()
    {
        // transform.up = up;
        // Quaternion targetRotation = Quaternion.FromToRotation(Vector3.up,up);
        // transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,25*Time.deltaTime);
        // Vector3 forward = transform.forward;
        // transform.up = -resultingForce.normalized;
        transform.rotation = Quaternion.FromToRotation(transform.up,-resultingForce)*transform.rotation;
    }
    public float GetGravity(){
        return gravity;
    }

    public bool IsInSpace(){
        return inSpace;
    }
}