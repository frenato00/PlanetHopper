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
    public float time;
    private float currentTime;
    private float reverseParam;
    private Vector3 deltaPosition;
    private Vector2 conveyorForce;
    
    private void activate()
    {
        if (!active)
        {
            Invoke(methodName, 0f);
            meshRenderer.material.color = Color.green;
            currentTime = 0;
        }
    }
    
    private void deactivate()
    {
        if (active)
        {
            Invoke(methodName, 0f);
            meshRenderer.material.color = Color.red;
        }
    }

    public void changeState()
    {
        if (!active)
        {
            activate();
        }
        else
        {
            deactivate();
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        active = false;
        meshRenderer = GetComponent<MeshRenderer>();
        currentTime = 0;
    }

    public void stopMoving()
    {
        if (!active)
        {
            deltaPosition = target.getDeltaPosition();
            target.setDeltaPosition(Vector3.zero);
        }
        else
        {
            target.setDeltaPosition(deltaPosition);
        }
        active = !active;
    }
    
    public void setMoveX()
    {
        if (!active)
        {
            Vector3 deltaPosition = target.getDeltaPosition();
            reverseParam = deltaPosition.x;
            deltaPosition.x = param;
            target.setDeltaPosition(deltaPosition);
        }
        else
        {
            Vector3 deltaPosition = target.getDeltaPosition();
            deltaPosition.x = reverseParam;
            target.setDeltaPosition(deltaPosition);
        }
        active = !active;
    }
    
    public void setMoveY()
    {
        if (!active)
        {
            Vector3 deltaPosition = target.getDeltaPosition();
            reverseParam = deltaPosition.y;
            deltaPosition.y = param;
            target.setDeltaPosition(deltaPosition);
        }
        else
        {
            Vector3 deltaPosition = target.getDeltaPosition();
            deltaPosition.y = reverseParam;
            target.setDeltaPosition(deltaPosition);
        }
        active = !active;
    }
    
    public void setMoveZ()
    {
        if (!active)
        {
            Vector3 deltaPosition = target.getDeltaPosition();
            reverseParam = deltaPosition.z;
            deltaPosition.z = param;
            target.setDeltaPosition(deltaPosition);
        }
        else
        {
            Vector3 deltaPosition = target.getDeltaPosition();
            deltaPosition.z = reverseParam;
            target.setDeltaPosition(deltaPosition);
        }
        active = !active;
    }
    
    public void setJumpForce()
    {
        if (!active)
        {
            reverseParam = target.getJumpForce();
            target.setJumpForce(param);
        }
        else
        {
            target.setJumpForce(reverseParam);
        }
        active = !active;
    }
    
    public void disableJumpPad()
    {
        if (!active)
        {
            reverseParam = target.getJumpForce();
            target.setJumpForce(0);
        }
        else
        {
            target.setJumpForce(reverseParam);
        }
        active = !active;
    }
    
    public void setConveyorForceX()
    {
        if (!active)
        {
            Vector2 conveyorForce = target.getConveyorForce();
            reverseParam = conveyorForce.x;
            conveyorForce.x = param;
            target.setConveyorForce(conveyorForce);
        }
        else
        {
            Vector2 conveyorForce = target.getConveyorForce();
            conveyorForce.x = reverseParam;
            target.setConveyorForce(conveyorForce);
        }
        active = !active;
    }
    
    public void setConveyorForceY()
    {
        if (!active)
        {
            Vector2 conveyorForce = target.getConveyorForce();
            reverseParam = conveyorForce.y;
            conveyorForce.y = param;
            target.setConveyorForce(conveyorForce);
        }
        else
        {
            Vector2 conveyorForce = target.getConveyorForce();
            conveyorForce.y = reverseParam;
            target.setConveyorForce(conveyorForce);
        }
        active = !active;
    }
    
    public void disableConveyorBelt()
    {
        if (!active)
        {
            conveyorForce = target.getConveyorForce();
            target.setConveyorForce(Vector2.zero);
        }
        else
        {
            target.setConveyorForce(conveyorForce);
        }
        active = !active;
    }
    
    public void turnDestructable()
    {
        if (!active)
        {
            target.turnDestructable();
        }
        else
        {
            target.turnPermanent();
        }
        active = !active;
    }
    
    public void turnPermanent()
    {
        if (!active)
        {
            target.turnPermanent();
        }
        else
        {
            target.turnDestructable();
        }
        active = !active;
    }

    void Update()
    {
        if (time > 0)
        {
            currentTime += Time.deltaTime;
            if (currentTime > time)
            {
                if (active)
                {
                    deactivate();
                }
                currentTime = 0;
            }
        }
    }
}
