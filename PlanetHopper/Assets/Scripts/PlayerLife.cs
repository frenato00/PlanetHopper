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
    float time = 0f;
    int enemiesKilled = 0;

    bool isDead = false;
    bool isTimeRunning = false;

    public GameObject playerUIPrefab;

    [HideInInspector]
    public GameObject playerUI;

    [Header("Sound Effects")]
    public FMODUnity.EventReference deathExhalesSFX;
    public FMODUnity.EventReference deathSuffocationSFX;
    public FMODUnity.EventReference damagedSFX;



    GameObject canvas;

    PlayerGravity playerGravity; 

    private void Start(){
        playerGravity = GetComponent<PlayerGravity>(); 
        health = maxHealth;
        oxygen = maxOxygen;
        
        isTimeRunning = true;

        playerUI = Instantiate(playerUIPrefab, transform);
        PlayerUI playerUIComp = playerUI.GetComponent<PlayerUI>();
        playerUIComp.playerLife = this;
        canvas = GameObject.Find("Canvas");
        playerUI.transform.SetParent(canvas.transform, false);
        
    }

    private void Update(){

        time= isTimeRunning ?  time + Time.deltaTime : time;

        if(!isDead && oxygen > 0 && playerGravity.IsInSpace()){
            oxygen -= Time.deltaTime;

            if(oxygen <= 0){
                FMODUnity.RuntimeManager.PlayOneShot(deathSuffocationSFX, transform.position);
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
        FMODUnity.RuntimeManager.PlayOneShot(damagedSFX, transform.position);
    }

    public IEnumerator TakeDamage()
    {
        if(isDead)
            yield return null;
        health -= 1;
        if(health <= 0)
        {   
            FMODUnity.RuntimeManager.PlayOneShot(deathExhalesSFX, transform.position);
            Die();
            yield return null;
        }

        yield return new WaitForSeconds(1);
    }


    void Die()
    {   
        StopTime();
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

    public float GetCurrentTime(){
        return time;
    }

    public void StartTime(){
        isTimeRunning = true;
    }

    public void StopTime(){
        isTimeRunning = false;
    }

    public void SetEnemiesKilled(int amount){
        enemiesKilled = amount;
    }

    public int GetEnemiesKilled(){
        return enemiesKilled;
    }

    public int AddEnemiesKilled(int amount){
        enemiesKilled += amount;
        return enemiesKilled;
    }

    public void Revive(){
        isDead = false;
        health = maxHealth;
        oxygen = maxOxygen;
        playerUI.SetActive(true);
        GetComponent<PlayerMovement>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        this.enabled = true;
        StartTime();
        
    }

    void onDisable(){
        Destroy(playerUI);
    }

}
