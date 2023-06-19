using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EndLevel : MonoBehaviour
{
    GameObject player;
    Rigidbody rb;
    
    public GameObject switchCameraPrefab;
    GameObject switchCameraObject;
    GameObject canvas;
    

    Vector3 startingPosition;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.endLevelTakeOff += OnEndingGame;
        rb = GetComponent<Rigidbody>();
        
        canvas = GameObject.Find("Canvas");
        switchCameraObject = Instantiate( switchCameraPrefab, new Vector3(0,0,0), Quaternion.identity);
        
    }

    void OnEndingGame(){
        // Make the object move forward at constant speed depending on time
        player = GameObject.FindGameObjectWithTag("Player");
        
        SwitchCamera switchCameraManager = switchCameraObject.GetComponent<SwitchCamera>();
        Debug.Log("SwitchCameraManager: " + switchCameraManager.name);
        switchCameraManager.Camera_2 = GameObject.Find("EndLevelObject").transform.Find("EndGameCamera").gameObject;
        Debug.Log("SwitchCameraManager.Camera_2: " + GameObject.Find("EndGameCamera"));
        switchCameraObject.transform.SetParent(canvas.transform, false);
        switchCameraManager.ChangeCamera();
        

        if(rb != null){
            startingPosition = rb.transform.position;
            rb.AddForce(Vector3.up * 10000f * Time.deltaTime);
        }

    }

    private IEnumerator ManageCameraCoroutine(){
        yield return new WaitForSeconds(5f);
        GameManager.instance.WinGameAfterSwitchCamera();
    }

    void OnDestroy(){
        //Destroy(switchCameraObject);
        Destroy(rb);
        Destroy(gameObject);
        GameManager.endLevelTakeOff -= OnEndingGame;
    }

}

