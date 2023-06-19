using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParticle : MonoBehaviour
{
    public ParticleSystem particleSystem;

    public GameObject spark;

    public string shooterTag;

    [HideInInspector]
    public float damage = 25f;

    List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    // Start is called before the first frame update
    private void OnParticleCollision(GameObject other){
        int events = particleSystem.GetCollisionEvents(other, collisionEvents);

        for (int i= 0; i< events; i++){
            if (collisionEvents[i].colliderComponent.CompareTag(shooterTag))
            {
                continue;
            }

            if (collisionEvents[i].colliderComponent.CompareTag("Switch"))
            {
                Switch hitSwitch = collisionEvents[i].colliderComponent.GetComponent<Switch>();
                hitSwitch.changeState();
                
            }
            IDamageable damageable = collisionEvents[i].colliderComponent.GetComponent<IDamageable>();
            damageable?.TakeDamage(damage);
            Instantiate(spark, collisionEvents[i].intersection, Quaternion.LookRotation(collisionEvents[i].normal));
        }
    }

    public void Play(){
        if(particleSystem != null){
            particleSystem.Play();
        }
    }
}
