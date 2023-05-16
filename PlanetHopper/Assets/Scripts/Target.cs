using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour, IDamageable
{
    private float health = 50f;
    // Start is called before the first frame update
    public void TakeDamage(float damage){
        health -= damage;
        if(health <= 0){
            Destroy(gameObject);
        }
    }

}
