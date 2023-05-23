using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityScriptDirectional : MonoBehaviour
{
    public float gravity;
    public Vector3 gravityDir;

    void OnTriggerStay(Collider other){
        GameObject obj = other.gameObject;
        if(obj.CompareTag("Player")||obj.CompareTag("Enemy")){
            PlayerGravity player = obj.GetComponent<PlayerGravity>();
            player.AddForce(gravityDir.normalized*gravity);
        }
    }
}
