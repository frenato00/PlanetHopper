using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


public class CheckpointManager : MonoBehaviour, ICheckpoint
{

    private Vector3 _playerPosition = new Vector3();

    IDictionary<GameObject, Vector3> _enemyPositions = new Dictionary<GameObject, Vector3>();
    IDictionary<GameObject, Vector3> _consumablePositions = new Dictionary<GameObject, Vector3>();

    public Vector3 spawnPosition;

    private int _health;
    private float _oxygen;
    private int _points;

    public Color activeColor = Color.green;
    public Color inactiveColor = Color.red;

    private bool _used = true;

    PlayerLife playerLife;

    public void Start()
    {
        playerLife = GameObject.FindWithTag("Player").GetComponent<PlayerLife>();
    }

    public void Update()
    {
        if(playerLife == null)
        {
            playerLife = GameObject.FindWithTag("Player").GetComponent<PlayerLife>();
        }

        if(GameManager.instance.currentCheckpoint == this){
            Debug.Log("Checkpoint active");
            SetActivated(true);
        }else{
            Debug.Log("Checkpoint not active");
            SetActivated(false);
        }

        if(_used == false && GameManager.instance.currentCheckpoint != this){
            this.gameObject.SetActive(false);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && _used)
        {
            SaveCheckpoint();
            _used = false;

        }
    }

    public void SaveCheckpoint()
    {    
        // Save the game state
        _playerPosition = GameObject.FindWithTag("Player").transform.position;

        // Save the positions of all enemies
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            _enemyPositions.Add(enemies[i], enemies[i].transform.position);
        }

        // Save the positions of all consumables
        GameObject[] consumables = GameObject.FindGameObjectsWithTag("Consumable");
        for (int i = 0; i < consumables.Length; i++)
        {
            _consumablePositions.Add(consumables[i], consumables[i].transform.position);
        }

        playerLife = GameObject.FindWithTag("Player").GetComponent<PlayerLife>();

        _health = playerLife.GetCurrentHealth();
        _oxygen = playerLife.GetCurrentOxygen();
        _points = playerLife.GetCurrentPoints();

        GameManager.instance.currentCheckpoint = this;        
    }

    public void ResetGameState()
    {
        // Go through all enemies, set them active and set their positions to the saved positions
        foreach (KeyValuePair<GameObject, Vector3> enemy in _enemyPositions)
        {
            GameObject enemyObject = enemy.Key;
            enemyObject.transform.position = enemy.Value;
            enemyObject.SetActive(true);
        }

        // Go through all consumables, set them active and set their positions to the saved positions
        foreach (KeyValuePair<GameObject, Vector3> consumable in _consumablePositions)
        {
            GameObject consumableObject = consumable.Key;
            consumableObject.transform.position = consumable.Value;
            consumableObject.SetActive(true);
        }


        GameObject player = GameObject.FindWithTag("Player");

        player.GetComponent<PlayerLife>().enabled = true;
        player.GetComponent<PlayerLife>().SetCurrentHealth(_health);
        player.GetComponent<PlayerLife>().RefillOxygen(100);
        player.GetComponent<PlayerLife>().SetCurrentPoints(_points);
        player.GetComponent<PlayerLife>().playerUI.SetActive(true);
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<Rigidbody>().isKinematic = false;

        player.GetComponent<Swing>().StopGrapple();

        player.transform.position = spawnPosition;


    }

    void SetActivated(bool active)
    {
        if (active)
        {
            GetComponent<MeshRenderer>().material.color = activeColor;
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = inactiveColor;
        }
    }


    
}