using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{

    public float sensitivityX;
    public float sensitivityY;
    public Transform orientation;

    float xRotation;
    float yRotation;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X")*Time.deltaTime * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y")*Time.deltaTime * sensitivityY;
        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // transform.forward = orientation.forward;
        // Quaternion targetRotation = Quaternion.FromToRotation(transform.up,orientation.up)* transform.rotation*Quaternion.Euler(xRotation, mouseX, 0);
        Quaternion targetRotation = orientation.rotation*Quaternion.Euler(xRotation, mouseX, 0);
        transform.rotation = targetRotation;
        // transform.rotation = Quaternion.Slerp(transform.rotation,targetRotation,25*Time.deltaTime);
        // transform.rotation = Quaternion.FromToRotation(transform.up, orientation.up);
        orientation.rotation *= Quaternion.Euler(0, mouseX, 0);
        // Vector3 forward = Vector3.Dot(transform.forward,orientation.forward)*orientation.forward;
        // Vector3 right = Vector3.Dot(transform.right,orientation.right)*orientation.right;
        // orientation.forward=forward;
        // orientation.right=right;

    }
}
