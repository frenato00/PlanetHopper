using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityScript : MonoBehaviour
{
    public PlayerGravity player;
    public float gravity;
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = player.GetComponent<Rigidbody>();
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
            // rb.drag=0.1f;
        }
    }

    void OnTriggerExit(Collider other){
        if(other.CompareTag("Player")){
            // rb.drag=0;
        }
    }
}