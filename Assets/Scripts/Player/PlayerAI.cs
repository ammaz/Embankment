using UnityEngine;
using UnityEngine.AI;

public class PlayerAI : MonoBehaviour
{
    //For Singleton
    public static PlayerAI instance;

    public NavMeshAgent agent;

    //For Enemy Position
    public Transform enemy;
    //For Enemy Outpost Position
    public Transform enemyOutpost;

    //LayerMask for identifying player and ground
    public LayerMask whatIsGround, whatIsEnemy, whatIsEnemyOutpost;

    //Patroling
    public Vector3 walkPoint;
    public bool walkPointSet;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public Transform shootingPoint;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, enemyOutpostInSightRange, enemyOutpostInAttackRange;

    //Player Animator
    private Animator playerAnim;

    //Player Select UI
    public GameObject playerSelectUI;

    private void Awake()
    {
        //For Singleton
        if (instance == null)
        {
            instance = this;
        }

        //Finding Player by its name in scene (Subject to change)
        agent = GetComponent<NavMeshAgent>();
        playerAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        //Check for sight and attack range of player
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsEnemy);
        //playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsEnemy);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsEnemy) && enemy != null;

        //Check for attack range of Outpost
        enemyOutpostInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsEnemyOutpost);
        enemyOutpostInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsEnemyOutpost);

        //Player State Checks
        if (!playerInSightRange && !playerInAttackRange && !enemyOutpostInSightRange && !enemyOutpostInSightRange && !walkPointSet) Idle();
        if (!playerInSightRange && !playerInAttackRange && !enemyOutpostInSightRange && !enemyOutpostInAttackRange && walkPointSet) PlayerGotoPosition();
        if ((playerInSightRange && !playerInAttackRange) || (enemyOutpostInSightRange && !enemyOutpostInAttackRange)) ChasePlayer();
        if ((playerInAttackRange && playerInSightRange) || (enemyOutpostInSightRange && enemyOutpostInAttackRange)) AttackPlayer();
    }

    //Player State Functions
    private void Idle()
    {
        //For Idle Animation
        playerAnim.SetBool("isShooting", false);
        playerAnim.SetBool("isWalking", false);
        playerAnim.SetBool("isIdle", true);

        //Make sure player doesn't move / to stop player when he is idle
        agent.SetDestination(transform.position);
    }

    private void ChasePlayer()
    {
        //For Walking Animation
        playerAnim.SetBool("isShooting", false);
        playerAnim.SetBool("isIdle", false);
        playerAnim.SetBool("isWalking", true);

        Collider[] rangeChecksEnemy = Physics.OverlapSphere(transform.position, sightRange, whatIsEnemy);
        Collider[] rangeChecksOutpost = Physics.OverlapSphere(transform.position, sightRange, whatIsEnemyOutpost);

        if (rangeChecksEnemy.Length != 0)
        {
            foreach(Collider c in rangeChecksEnemy)
            {
                if (c != null)
                {
                    enemy = rangeChecksEnemy[0].transform;
                    agent.SetDestination(enemy.position);
                    break;
                }
                else
                {
                    continue;
                }
            }     
        }
        else if (rangeChecksOutpost.Length != 0)
        {
            foreach (Collider c in rangeChecksOutpost)
            {
                if (c != null)
                {
                    enemyOutpost = rangeChecksOutpost[0].transform;
                    agent.SetDestination(enemyOutpost.position);
                    break;
                }
                else
                {
                    continue;
                }
            }  
        }
        else
        {
            //Do Nothing
        }
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move/To stop enemy while firing bullets
        agent.SetDestination(transform.position);

        //Enemy will look at player or Object or Outpost
        if (enemy != null && playerInAttackRange)
        {
            transform.LookAt(enemy);
        }
        else if (enemyOutpost != null && enemyOutpostInAttackRange)
        {
            transform.LookAt(enemyOutpost);
        }
        else
        {
            //Do Nothing
        }

        if (!alreadyAttacked)
        {
            ///Attack code here
            //For Shooting Animation
            playerAnim.SetBool("isIdle", false);
            playerAnim.SetBool("isWalking", false);
            playerAnim.SetBool("isShooting", true);

            //Enemy Shooting Sound

            Rigidbody rb = Instantiate(projectile, shootingPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void PlayerGotoPosition()
    {
            //For Walking Animation
            playerAnim.SetBool("isShooting", false);
            playerAnim.SetBool("isIdle", false);
            playerAnim.SetBool("isWalking", true);

            agent.SetDestination(walkPoint);
                
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
            walkPoint = transform.position;
        }          
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
