using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    public float maxHealth = 50f;
    private float health;
    public AudioSource killAudio;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage){
        health -= damage;
        if(health <= 0){
            
            //disable the object
            gameObject.SetActive(false);
            
        }
    }

    public void Reset(){
        health = maxHealth;
        gameObject.SetActive(true);
    }

}
