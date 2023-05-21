using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
    private bool active = false;
    private MeshRenderer meshRenderer;
    public Platform target;
    public string methodName;
    public float param;
    
    public void activate()
    {
        if (!active)
        {
            Invoke(methodName, 0f);
            meshRenderer.material.color = Color.green;
            active = true;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        active = false;
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void stopMoving()
    {
        target.setDeltaPosition(Vector3.zero);
    }
    
    public void setMoveX()
    {
        Vector3 deltaPosition = target.getDeltaPosition();
        deltaPosition.x = param;
        target.setDeltaPosition(deltaPosition);
    }
    
    public void setMoveY()
    {
        Vector3 deltaPosition = target.getDeltaPosition();
        deltaPosition.y = param;
        target.setDeltaPosition(deltaPosition);
    }
    
    public void setMoveZ()
    {
        Vector3 deltaPosition = target.getDeltaPosition();
        deltaPosition.z = param;
        target.setDeltaPosition(deltaPosition);
    }
    
    public void setJumpForce()
    {
        target.setJumpForce(param);
    }
    
    public void disableJumpPad()
    {
        target.setJumpForce(0);
    }
    
    public void setConveyorForceX()
    {
        Vector2 conveyorForce = target.getConveyorForce();
        conveyorForce.x = param;
        target.setConveyorForce(conveyorForce);
    }
    
    public void setConveyorForceY()
    {
        Vector2 conveyorForce = target.getConveyorForce();
        conveyorForce.y = param;
        target.setConveyorForce(conveyorForce);
    }
    
    public void disableConveyorBelt()
    {
        target.setConveyorForce(Vector2.zero);
    }
    
    public void turnDestructable()
    {
        target.turnDestructable();
    }
    
    public void turnPermanent()
    {
        target.turnPermanent();
    }
}
