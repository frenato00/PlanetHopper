using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;


public class CheckpointManager : MonoBehaviour, ICheckpoint
{
    private Vector3 _playerPosition = new Vector3();

    IDictionary<GameObject, Vector3> _enemyPositions = new Dictionary<GameObject, Vector3>();
    IDictionary<GameObject, Vector3> _consumablePositions = new Dictionary<GameObject, Vector3>();

    public Vector3 spawnPosition;

    private int _health;
    private float _oxygen;
    private int _points;
    private int _enemiesKilled;

    GameObject player;
    GameObject playerParent;

    public Material activeCheckpointMaterial;

    private bool _used = true;

    PlayerLife playerLife;


    public void Update()
    {
        if(playerLife == null || playerParent == null || player == null)
        {
            player = GameObject.FindWithTag("Player");
            playerLife = player?.GetComponent<PlayerLife>();
            playerParent = player?.transform.parent.gameObject;
            
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

        if(playerLife == null || playerParent == null || player == null)
        {
            playerLife = GameObject.FindWithTag("Player").GetComponent<PlayerLife>();
            player = GameObject.FindWithTag("Player");
            playerParent =  GameObject.FindWithTag("Player").transform?.parent.gameObject;
        }

        _health = playerLife.GetCurrentHealth();
        _oxygen = playerLife.GetCurrentOxygen();
        _points = playerLife.GetCurrentPoints();
        _enemiesKilled = playerLife.GetEnemiesKilled();

        GameManager.instance.currentCheckpoint = this; 

        GetComponent<MeshRenderer>().material = activeCheckpointMaterial;      
    }

    public void ResetGameState()
    {
        // Go through all enemies, set them active and set their positions to the saved positions
        foreach (KeyValuePair<GameObject, Vector3> enemy in _enemyPositions)
        {
            GameObject enemyObject = enemy.Key;
            enemyObject.transform.position = enemy.Value;
            enemyObject.GetComponent<Target>().Reset();
        }

        // Go through all consumables, set them active and set their positions to the saved positions
        foreach (KeyValuePair<GameObject, Vector3> consumable in _consumablePositions)
        {
            GameObject consumableObject = consumable.Key;
            consumableObject.transform.position = consumable.Value;
            consumableObject.SetActive(true);
        }

        playerParent.SetActive(true);
        player.GetComponent<Swing>().StopGrapple();
        player.GetComponent<PlayerLife>().SetCurrentPoints(_points);
        player.GetComponent<PlayerLife>().SetEnemiesKilled(_enemiesKilled);
        player.GetComponent<PlayerLife>().Revive();


        player.transform.position = spawnPosition;

    }

    
}