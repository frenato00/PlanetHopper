using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour, IDamageable
{
    public int maxHealth = 3;
    public int maxOxygen = 10;
    int oxygen = 0;
    int health = 0;
    int points = 0;
    bool isDead = false;
    Coroutine ReduceOxygenCoroutine = null;

    public GameObject playerUIPrefab;
    GameObject playerUI;

    GameObject canvas;

    PlayerGravity playerGravity; 

    private void Start(){
        playerGravity = GetComponent<PlayerGravity>(); 
        health = maxHealth;
        oxygen = maxOxygen;
        playerUI = Instantiate(playerUIPrefab, transform);
        PlayerUI playerUIComp = playerUI.GetComponent<PlayerUI>();
        playerUIComp.playerLife = this;
        canvas = GameObject.Find("Canvas");
        playerUI.transform.SetParent(canvas.transform, false);
        
    }

    private void Update(){
        if(!isDead && oxygen > 0 && playerGravity.IsInSpace()){
            if(ReduceOxygenCoroutine == null){
                ReduceOxygenCoroutine = StartCoroutine(ReduceOxygen());
            }
        }

        if(!playerGravity.IsInSpace()){
            if(ReduceOxygenCoroutine != null){
                StopCoroutine(ReduceOxygenCoroutine);
                ReduceOxygenCoroutine = null;
            }
            if(oxygen < maxOxygen){
                RefillOxygen(maxOxygen);
            }
        }

        Debug.Log("Oxygen: " + oxygen);

        
    }

    public void TakeDamage(float damage)
    {
        StartCoroutine(TakeDamage());
    }

    public IEnumerator TakeDamage()
    {
        health -= 1;
        if(health <= 0)
        {   
            Die();
            yield return null;
        }

        yield return new WaitForSeconds(1);
    }

    IEnumerator ReduceOxygen()
    {
        while(true){
            if(oxygen <= 0){
                Die();
                yield return null;
            }
            yield return new WaitForSeconds(1);
            oxygen -= 1;
        }
    }
    

    void Die()
    {   

        StopAllCoroutines();
        Destroy(playerUI);
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        isDead = true;
        GameManager.instance.GameOver();

    }

    public bool GainHealth(int amount = 1)
    {
        if(health < maxHealth)
        {
            health += 1;
            return true;
        }

        return false;
    }

    public bool RefillOxygen(int amount)
    {
        oxygen += amount;
        oxygen = oxygen > maxOxygen ? maxOxygen : oxygen;

        return true;
    }

    public void GainPoints(int amount)
    {
        points += amount;
    }

    public int GetCurrentHealth()
    {
        return health;
    }

    public int GetCurrentOxygen()
    {
        return oxygen;
    }

    public int GetCurrentPoints()
    {
        return points;
    }
}
