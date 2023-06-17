using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public BulletParticle bulletParticle;

    public void Shoot(){
        bulletParticle.Play();
    }
}
