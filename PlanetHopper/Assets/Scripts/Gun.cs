using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{

    [Header("References")]
    [SerializeField] GunData gunData;
    [SerializeField] Transform cam;

    float timeSinceLastShot;

    private void Start(){
        PlayerShoot.shootInput += Shoot;
    }


    private bool CanShoot() => timeSinceLastShot > 1f/ (gunData.fireRate/60f);

    private void Shoot(){
        if(CanShoot()){
            if(Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, gunData.maxDistance)){
                IDamageable damageable = hit.transform.GetComponent<IDamageable>();
                damageable?.TakeDamage(gunData.damage);
            }

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
        // Play animation
    }
}
