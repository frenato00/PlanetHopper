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
    public float jumpForce = 10f;
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
    private bool returning;
    private bool destroying;
    private bool destroyed = false;
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
            transform.position = currentPosition;
        
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
}