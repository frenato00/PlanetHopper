using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityScript : MonoBehaviour
{
    public PlayerGravity player;
    public float gravity;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerStay(Collider other){
        if(other.CompareTag("Player")){
            Vector3 dir = transform.position - player.transform.position;
            player.AddForce(dir.normalized*gravity);
            // Debug.Log(gravity);
            player.GetComponent<Rigidbody>().drag=0.1f;
        }
    }

    void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
            player.GetComponent<Rigidbody>().drag=0;
        }
    }
}
