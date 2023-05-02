using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public bool isDestructable = false;
    public bool isJumpPad = false;
    public bool isConveyorBelt = false;
    public Vector3 deltaPosition = new Vector3(0, 0, 0);
    public float movingTime = 0;
    public float waitingTime = 0;
    public float jumpForce = 10f;
    public Vector2 conveyorForce = new Vector2(5f, 5f);
    public GameObject gameObject;

    private Transform transform;
    private BoxCollider collider;
    private MeshRenderer meshRenderer;
    private Material[] materials;
    private Vector3 startingPosition;
    private Vector3 finalPosition;
    private bool returning;
    private bool destroying;
    private float currentTime;
    private float currentAlpha;
    private float disappearTime = 2;

    void Start()
    {
        destroying = false;
        returning = false;
        transform = GetComponent<Transform>();
        collider = GetComponent<BoxCollider>();
        meshRenderer = GetComponent<MeshRenderer>();
        startingPosition = transform.position;
        finalPosition = startingPosition + deltaPosition;
        currentAlpha = 1;
        UpdateAlpha(currentAlpha);
        currentTime = 0;
    }

    private void UpdateAlpha(float newAlpha)
    {
        materials = meshRenderer.materials;
        //materials[0].color = new Color(materials[0].color.r, materials[0].color.g, materials[0].color.b,  newAlpha);
        materials[0].color = new Color(newAlpha, newAlpha, newAlpha,  newAlpha);
        meshRenderer.materials = materials;
    }
    
    void Update()
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
        
        transform.position = currentPosition;
        
        if (destroying)
        {
            currentAlpha -= Time.deltaTime / disappearTime;
            UpdateAlpha(currentAlpha);
            if (currentAlpha <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.CompareTag("Player"))
        {
            if (isDestructable)
            {
                destroying = true;
            }

            if (isJumpPad)
            {
                collision.rigidbody.AddForce(transform.up.normalized * jumpForce, ForceMode.Impulse);
            }
            
            if (isConveyorBelt)
            {
                collision.rigidbody.AddForce(transform.right.normalized * conveyorForce.x + transform.forward.normalized * conveyorForce.y, ForceMode.Impulse);
            }
        }
    }
}