using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public BulletParticle bulletParticle;

    public FMODUnity.EventReference shootSFX;

    public void Shoot(){

        bulletParticle.Play();
        FMODUnity.RuntimeManager.PlayOneShot(shootSFX, transform.position);
    }
}
