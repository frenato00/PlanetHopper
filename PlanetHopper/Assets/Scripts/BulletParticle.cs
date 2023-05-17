using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParticle : MonoBehaviour
{
    public ParticleSystem particleSystem;

    public GameObject spark;

    List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

    // Start is called before the first frame update
    private void OnParticleCollision(GameObject other){
        int events = particleSystem.GetCollisionEvents(other, collisionEvents);

        for (int i= 0; i< events; i++){

            IDamageable damageable = collisionEvents[i].colliderComponent.GetComponent<IDamageable>();
            damageable?.TakeDamage(50f);
            Instantiate(spark, collisionEvents[i].intersection, Quaternion.LookRotation(collisionEvents[i].normal));
        }
    }

    public void Play(){
        particleSystem.Play();
    }
}
