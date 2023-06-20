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
            
            //disable the object
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
