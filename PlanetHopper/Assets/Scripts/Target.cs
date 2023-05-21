using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    public float health = 50f;
    public AudioSource killAudio;
    // Start is called before the first frame update
    public void TakeDamage(float damage){
        health -= damage;
        if(health <= 0){
            
            Destroy(gameObject);
            
        }
    }

}
