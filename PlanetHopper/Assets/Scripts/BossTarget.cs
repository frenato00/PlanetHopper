using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class BossTarget : MonoBehaviour, IDamageable
{
    public float maxHealth = 5000f;

    public FMODUnity.EventReference hitSFX;

    private BossAI bossAI;

    public GameObject bossUIPrefab;

    private GameObject bossUI;
    
    private float health;

    private int timesExploded = 0;
    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        bossAI = GetComponent<BossAI>();

        bossUI = Instantiate(bossUIPrefab, transform);
        bossUI.GetComponent<BossUI>().bossTarget = this;

        bossUI.transform.SetParent(GameObject.Find("Canvas").transform, false);        
    }

    public void TakeDamage(float damage){

        if( !CanTakeDamage()){
            return;
        }

        health -= damage;

        FMODUnity.RuntimeManager.PlayOneShot(hitSFX, transform.position);

        if(health <= 0){
            
            gameObject.SetActive(false);
            if(gameObject.CompareTag("Enemy") || gameObject.CompareTag("Finish")){
                GameObject.FindWithTag("Player").GetComponent<PlayerLife>().AddEnemiesKilled(1);      
                GameManager.instance.Win();    

            }
        }

        if(health <= 5*maxHealth/6 && timesExploded == 0){
            bossAI.PrepareExplodePlanet();
            timesExploded++;
        }

        if(health <= 4*maxHealth/6 && timesExploded == 1){
            bossAI.PrepareExplodePlanet();
            timesExploded++;
        }

        if(health <= 3*maxHealth/6 && timesExploded == 2){
            bossAI.PrepareExplodePlanet();
            timesExploded++;
        }

        if(health <= 2*maxHealth/6 && timesExploded == 3){
            bossAI.PrepareExplodePlanet();
            timesExploded++;
        }

        if(health <= maxHealth/6 && timesExploded == 4){
            bossAI.PrepareExplodePlanet();
            timesExploded++;
        }

    }

    public void Reset(){
        health = maxHealth;
        gameObject.SetActive(true);
    }

    private bool CanTakeDamage(){
        return bossAI.GetBossState() == BossAI.BossState.Unshielded;
    }

    public bool IsShielded(){
        return bossAI.GetBossState() == BossAI.BossState.Shielded || bossAI.GetBossState() == BossAI.BossState.Exploding;
    }

    public float GetHealth(){
        return health;
    } 

    

}
