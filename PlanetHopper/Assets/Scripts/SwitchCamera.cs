using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This class is responsible for switching the camera from the main camera to the camera that shows the player the win screen.
 * NOT BEING USED
 */


public class SwitchCamera : MonoBehaviour , ISwitchCamera
{
    //Cam 1 is the starting Camera
    GameObject Camera_1;
    public GameObject Camera_2;
    public int Manager = 0;

    GameObject player;

    void Start(){
        Camera_1 = GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.FindGameObjectWithTag("Player").transform.parent.gameObject;
    }

    public void ChangeCamera(){
        GetComponent<Animator>().SetTrigger("Change");

    }

    public void ManageCamera(){
        if(Camera_1 == null || Camera_2 == null){
            return;
        }
        if(Manager == 0){
            Cam_2();
        }
        else{
            Cam_1();
        }

        StartCoroutine(ManageCameraCoroutine());
    }

    public void Cam_1(){
        player.SetActive(true);
        Camera_2.SetActive(false);
        Manager = 0;
    }

    public void Cam_2(){
        Destroy(player);
        Camera_2.SetActive(true);
        Manager = 1;
    }

    private IEnumerator ManageCameraCoroutine(){
        yield return new WaitForSeconds(5f);
        Debug.Log("Switching to win screen");
        GameManager.instance.WinGameAfterSwitchCamera();
    }


    // Update is called once per frame
    void Update()
    {
        if(Camera_1 != null){
            return;
        }
        Camera_1 = GameObject.FindGameObjectWithTag("MainCamera");
        
    }


}
