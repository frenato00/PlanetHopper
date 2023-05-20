using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject spawnObject;
    private Vector3 spawnPoint;
    public float spawnRate = 0;
    private float timeTilNextSpawn;
    int x = 0;
    float timer = 0;

    void Start()
    {
        timer = 0;
        if (spawnObject == null)
        {
            Debug.LogError("SpawnObject is null");
        }

        spawnPoint = this.transform.position;

        Instantiate(spawnObject, spawnPoint, Quaternion.identity);

        if(spawnRate != 0) timeTilNextSpawn = 1 / spawnRate;
        
    }

    private void Update()
    {
        if(spawnRate == 0) return;
        timer += Time.deltaTime;
        Spawn();
    }

    void Spawn()
    {

        if (timer >= timeTilNextSpawn)
        {
            Instantiate(spawnObject, spawnPoint, Quaternion.identity);
            timer = 0;
        }
    }
}
