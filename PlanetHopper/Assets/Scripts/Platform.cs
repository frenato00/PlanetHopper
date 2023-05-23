using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool isDestructable = false;
    public Vector3 deltaPosition = new Vector3(0, 0, 0);
    public float movingTime = 0;
    public float waitingTime = 0;
    public float restartTime = 0;
    public float jumpForce = 0;
    public Vector2 conveyorForce = new Vector2(5f, 5f);
    public GameObject gameObject;

    private Swing grapplingObject;
    private Transform transform;
    private BoxCollider collider;
    private MeshRenderer meshRenderer;
    private Rigidbody rb;
    private Material[] materials;
    private Vector3 startingPosition;
    private Vector3 finalPosition;
    private Vector3 deltaGrapplePosition;
    private RaycastHit hitRay;
    private bool returning;
    private bool destroying;
    private bool destroyed = false;
    private bool moving = false;
    private float currentTime;
    private float currentAlpha;
    private float disappearTime = 2;

    void Start()
    {
        destroying = false;
        returning = false;
        rb = GetComponent<Rigidbody>();
        transform = GetComponent<Transform>();
        collider = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        startingPosition = transform.position;
        finalPosition = startingPosition + deltaPosition;
        currentAlpha = 1;
        grapplingObject = null;
        currentTime = 0;
    }

    public void setGrapple(Swing grappling)
    {
        grapplingObject = grappling;
        deltaGrapplePosition = grapplingObject.getSwingPoint() - transform.position;
    }
    
    public void unsetGrapple()
    {
        grapplingObject = null;
    }
    
    public Vector3 getDeltaPosition()
    {
        return deltaPosition;
    }
    
    public Vector2 getConveyorForce()
    {
        return conveyorForce;
    }
    
    public float getJumpForce()
    {
        return jumpForce;
    }
    
    public void setDeltaPosition(Vector3 newPosition)
    {
        deltaPosition = newPosition;
        finalPosition = startingPosition + deltaPosition;
        movingTime = 2;
        waitingTime = 1;
    }
    
    public void setConveyorForce(Vector2 newForce)
    {
        conveyorForce = newForce;
    }
    
    public void setJumpForce(float newForce)
    {
        jumpForce = newForce;
    }

    
    public void turnDestructable()
    {
        isDestructable = true;
        disappearTime = 2;
        restartTime = 5;
    }
    
    public void turnPermanent()
    {
        isDestructable = false;
    }
    
    private IEnumerator Restart()
    {
        UpdateAlpha(0);
        rb.detectCollisions = false;
        meshRenderer.enabled = false;
        transform.position = startingPosition;
        yield return new WaitForSeconds(restartTime);
        Start();
        rb.detectCollisions = true;
        meshRenderer.enabled = true;
        UpdateAlpha(1);
        destroyed = false;
    }

    private void UpdateAlpha(float newAlpha)
    {
        Color color = meshRenderer.material.color;
        color.a = newAlpha;
        meshRenderer.material.color = color;
    }
    
    void Update()
    {
        if (!destroyed)
        {
            if (deltaPosition != Vector3.zero)
            {
                if (currentTime >= (movingTime + waitingTime))
                {
                    returning = !returning;
                    currentTime = 0;
                }
        
                currentTime += Time.deltaTime;
                Vector3 currentPosition = transform.position;
        
                if (returning)
                {
                    currentPosition = Vector3.Lerp(finalPosition, startingPosition, currentTime / movingTime);
                }
                else
                {
                    currentPosition = Vector3.Lerp(startingPosition, finalPosition, currentTime / movingTime);
                }

                if (grapplingObject)
                {
                    grapplingObject.setSwingPoint(currentPosition + deltaGrapplePosition);
                }

                if (transform.position == currentPosition)
                {
                    moving = false;
                }
                else
                {
                    moving = true;
                    transform.position = currentPosition;
                }
            }
            
            if (destroying)
            {
                currentAlpha -= Time.deltaTime / disappearTime;
                UpdateAlpha(currentAlpha);
                if (currentAlpha <= 0)
                {
                    destroyed = true;
                    StartCoroutine(Restart());
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!destroyed)
        {
            if(collision.collider.CompareTag("Player"))
            {
                Physics.Raycast(collision.collider.transform.position, - collision.collider.transform.up, out hitRay);
                if (hitRay.collider.gameObject == gameObject)
                {
                    collision.gameObject.transform.SetParent(transform, true);
                }
                
                if (isDestructable)
                {
                    destroying = true;
                }

                if (jumpForce > 0)
                {
                    collision.rigidbody.AddForce(transform.up.normalized * jumpForce, ForceMode.Impulse);
                }
            
                if (conveyorForce != Vector2.zero)
                {
                    collision.rigidbody.AddForce(transform.right.normalized * conveyorForce.x + transform.forward.normalized * conveyorForce.y, ForceMode.Impulse);
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.gameObject.transform.parent = null;
        }
    }
}