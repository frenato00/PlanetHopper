using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    public float maxHealth = 50f;

    public FMODUnity.EventReference hitSFX;

    private float health;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(float damage){
        health -= damage;

        FMODUnity.RuntimeManager.PlayOneShot(hitSFX, transform.position);

        if(health <= 0){
            
            //disable the object
            //TODO: Add particle effect
            gameObject.SetActive(false);
            if(gameObject.CompareTag("Enemy")){
                GameObject.FindWithTag("Player").GetComponent<PlayerLife>().AddEnemiesKilled(1);          

            }
        }
    }

    public void Reset(){
        health = maxHealth;
        gameObject.SetActive(true);
    }

}
