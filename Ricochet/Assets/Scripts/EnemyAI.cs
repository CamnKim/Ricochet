using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;
    AudioSource enemyAudio;

    public LayerMask whatIsGround, whatIsPlayer;
    public int health = 100;
    public int damage, ricochets, scoreValue, points;
    public GameObject ScoreBox;
    public bool won = false;
    public AudioClip gunshot;


    public Transform attackPoint;
    public GameObject projectile;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;


    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
        ScoreBox = GameObject.FindGameObjectWithTag("Win");
        enemyAudio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (won) return;
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet) agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // walk point reached
        if (distanceToWalkPoint.magnitude < 1f) walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y - 1, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
        {
            walkPointSet = true;
        }
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(new Vector3(player.transform.position.x, player.transform.position.y + 1, player.transform.position.z));

        if (!alreadyAttacked)
        {
            GameObject currentBullet = Instantiate(projectile, attackPoint.position, Quaternion.identity);
            currentBullet.GetComponent<Ricochet>().numRicochets = ricochets;
            currentBullet.GetComponent<Ricochet>().damage = damage;
            currentBullet.GetComponent<Ricochet>().maxRicochet = ricochets;

            enemyAudio.clip = gunshot;
            enemyAudio.Play();

            currentBullet.transform.forward = transform.forward.normalized;
            currentBullet.GetComponent<Rigidbody>().AddForce(transform.forward.normalized * 20, ForceMode.Impulse);

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        ScoreBox.GetComponent<Score>().score += points;
        Destroy(gameObject);
        player.GetComponent<PlayerHealth>().currentHealth += 10;
    }

    public void scoreHit(int ricochets, int maxRicochets)
    {
        Debug.Log("Max: " + maxRicochets.ToString());
        Debug.Log("num: " + ricochets.ToString());
        
        points = (maxRicochets - ricochets + 1) * scoreValue;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
