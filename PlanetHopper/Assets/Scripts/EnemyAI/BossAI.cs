using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    public Transform orientation;
    Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    
    EnemyShoot enemyShoot;

    //Patrolling
    Vector3 walkPoint;
    bool walkPointSet=false;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    // public GameObject projectile;
    Rigidbody rb;

    //States
    public float sightRange, attackRange, speed;
    bool playerInSightRange, playerInAttackRange;

    [Header("Game Objects")]
    public GameObject[] shieldGenerators;
    public GameObject[] destroyablePlanets;
    public GameObject shieldObject;
    private LineRenderer deathRay;
    private DialogueTriggerMultiple dialogueTrigger;

    private bool canAttack = true;

    public enum BossState{
        Shielded,
        Unshielded,
        Exploding,
        Dead
    }

    private float time = 0;

    private float rotationSpeed = 1f;

    BossState bossState = BossState.Shielded;

    private void Start() {
        player = GameObject.FindWithTag("Player")?.transform;
        rb = GetComponent<Rigidbody>();
        enemyShoot = GetComponentInChildren<EnemyShoot>();
        deathRay = GetComponent<LineRenderer>();
        dialogueTrigger = GetComponent<DialogueTriggerMultiple>();
    }

    private void Update() {
        //Check for sight and attack range
        if(!player){
            player = GameObject.FindWithTag("Player").transform;
            return;
        }
        if(!enemyShoot){
            enemyShoot = GetComponentInChildren<EnemyShoot>();
            return;
        }
        if(!rb){
            rb = GetComponent<Rigidbody>();
            return;
        }
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();

        //Boss Stuff
        if(bossState == BossState.Shielded){
            verifyShieldGenerators();
        }

    }

    private void Patrolling() {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            MoveTo(walkPoint);
        
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 2f)
            walkPointSet = false;
    }

    private void SearchWalkPoint() {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = transform.position + randomX*transform.forward + randomZ*transform.right;
        RaycastHit hit;
        if (Physics.Raycast(walkPoint, -transform.up, out hit, 10f, whatIsGround))
            walkPoint= hit.point;
            walkPointSet = true;
    }

    private void ChasePlayer() {
        walkPointSet = false;
        MoveTo(player.position);

    }

    private void AttackPlayer() {
        walkPointSet = false;
        Transform objective = canAttack ? player : destroyablePlanets[0].transform;
        Quaternion rot = Quaternion.LookRotation(objective.position - orientation.position);
        rot *= Quaternion.Euler(Vector3.up * -90f);

        float deltaAngle = Quaternion.Angle(orientation.rotation, rot);

        orientation.rotation = Quaternion.Slerp(orientation.rotation, rot, time);
        time += Time.deltaTime * rotationSpeed ;


        // Add 90 degrees to the rotation so the enemy is facing the player

        if (!alreadyAttacked && canAttack ) {
            enemyShoot.Shoot();
            // Attack code here
            // Rigidbody rb = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
            // rb.AddForce(orientation.forward * 32f, ForceMode.Impulse);
            // rb.AddForce(transform.up * 8f, ForceMode.Impulse);


            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack() {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    void MoveTo(Vector3 walkPoint){
        Vector3 dir = (walkPoint-transform.position).normalized;
        dir = Vector3.Dot(dir,transform.forward)*transform.forward+Vector3.Dot(dir,transform.right)*transform.right;
        if (rb.velocity.magnitude<speed){
            rb.AddForce(dir * speed * 10f, ForceMode.Force);
        }
    }


    //-----------------BOSS STUFF-----------------//

    public BossState GetBossState(){
        return bossState;
    }

    public bool verifyShieldGenerators(){
        foreach(GameObject generator in shieldGenerators){
            if(generator.activeSelf){
                return false;
            }
        }
        TakeDownShield();
        return true;
    }

    public void TakeDownShield(){
        shieldObject.SetActive(false);
        bossState = BossState.Unshielded;
    }

    public void PrepareExplodePlanet(){
        bossState = BossState.Exploding;
        shieldObject.SetActive(true);
        canAttack = false;

        int closestPlanetID = FindClosestPlanet();

        deathRay.SetPosition(0, transform.position);
        deathRay.SetPosition(1, destroyablePlanets[closestPlanetID].transform.position);
        deathRay.enabled = true;

        StartCoroutine(ExplodePlanet(closestPlanetID));


    }

    private int FindClosestPlanet(){
        int closestPlanet = 0;
        float closestDistance = 1000000f;
        for(int i = 0; i < destroyablePlanets.Length; i++){
            float distance = Vector3.Distance(player.position, destroyablePlanets[i].transform.position);
            if(distance < closestDistance){
                closestDistance = distance;
                closestPlanet = i;
            }
        }
        return closestPlanet;
    }

    public IEnumerator ExplodePlanet(int closestPlanetID){
        // TODO - Add Light Animation to planet
        //TODO - Add sound effect to ray
        yield return new WaitForSeconds(8f);
        
        // get first planet
        GameObject planet = destroyablePlanets[closestPlanetID];



        // Destroy the planet
        planet.GetComponent<IDamageable>().TakeDamage(100000f);
        destroyablePlanets = destroyablePlanets.Where(val => val != planet).ToArray();
        
        deathRay.enabled = false;
        bossState = BossState.Unshielded;
        shieldObject.SetActive(false);
        canAttack = true;
        if(destroyablePlanets.Length==5)dialogueTrigger.TriggerDialogue(1);
    } 


}
