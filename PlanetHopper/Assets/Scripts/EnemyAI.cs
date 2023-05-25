using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform orientation;
    Transform player;
    public LayerMask whatIsGround, whatIsPlayer;
    public float health;

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

    private void Start() {
        player = GameObject.FindWithTag("Player").transform;
        rb = GetComponent<Rigidbody>();
        enemyShoot = GetComponentInChildren<EnemyShoot>();
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
        Debug.Log(walkPointSet);
        Debug.Log(walkPoint);
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);
        if (!playerInSightRange && !playerInAttackRange) Patrolling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
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
        orientation.LookAt(player);

        if (!alreadyAttacked) {
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

    public void TakeDamage(int damage) {
        health -= damage;
        if (health <= 0) {
            Invoke(nameof(DestroyEnemy), .5f);
        }
    }

    public void DestroyEnemy() {
        Destroy(gameObject);
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
}
