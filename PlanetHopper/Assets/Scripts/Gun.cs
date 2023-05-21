using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Gun : MonoBehaviour
{

    [Header("References")]
    [SerializeField] GunData gunData;
    [SerializeField] Transform cam;
    [SerializeField] BulletParticle bulletParticle;

    float timeSinceLastShot;


    private void Start(){
        PlayerShoot.shootInput += Shoot;
    }


    private bool CanShoot() => timeSinceLastShot > 1f/ (gunData.fireRate/60f);

    private void Shoot(){
        if(CanShoot()){
            timeSinceLastShot = 0f;
            OnGunShot();
        }

    }

    private void Update(){
        timeSinceLastShot += Time.deltaTime;
        Debug.DrawRay(cam.position, cam.forward * gunData.maxDistance, Color.red);
    }

    private void OnGunShot(){
        
        // Play sound
        // Play particle effect
        bulletParticle.Play();
        // Play animation
    }
}
