using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour, IDamageable
{
    public int maxHealth = 3;
    public int maxOxygen = 10;
    float oxygen = 0f;
    int health = 0;
    int points = 0;
    bool isDead = false;

    public GameObject playerUIPrefab;

    [HideInInspector]
    public GameObject playerUI;

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
            oxygen -= Time.deltaTime;

            if(oxygen <= 0){
                Die();
            }
        }

        if(!playerGravity.IsInSpace()){
            if(oxygen < maxOxygen){
                RefillOxygen(maxOxygen);
            }
        }


        
    }

    public void TakeDamage(float damage)
    {
        if(isDead)
            return;
        StartCoroutine(TakeDamage());
    }

    public IEnumerator TakeDamage()
    {
        if(isDead)
            yield return null;
        health -= 1;
        if(health <= 0)
        {   
            Die();
            yield return null;
        }

        yield return new WaitForSeconds(1);
    }


    void Die()
    {   
        isDead = true;
        StopAllCoroutines();
        playerUI.SetActive(false);
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GameManager.instance.GameOver();
        this.enabled = false;

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

    public float GetCurrentOxygen()
    {
        return oxygen;
    }

    public int GetCurrentPoints()
    {
        return points;
    }

    public void SetCurrentPoints(int amount)
    {
        points = amount;
    }

    public void SetCurrentHealth(int amount)
    {
        health = amount;
    }

}
