using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityScript : MonoBehaviour
{
    public float gravity;

    void OnTriggerStay(Collider other){
        GameObject obj = other.gameObject;
        if(obj.CompareTag("Player")||obj.CompareTag("Enemy")){
            PlayerGravity player = obj.GetComponent<PlayerGravity>();
            Vector3 dir = transform.position - player.transform.position;
            player.AddForce(dir.normalized*gravity);
        }
    }
}
