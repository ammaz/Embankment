using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.GlobalIllumination;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    //For Player Position
    public Transform player;
    //For Object Position
    public Transform Object;
    //For Outpost Position
    public Transform Outpost;

    //For GOTO Outpost Position (For Enemy Only)
    public Transform OutpostPosition;

    //LayerMask for identifying player and ground
    public LayerMask whatIsGround, whatIsPlayer, whatIsObject, whatIsPlayerOutpost;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public Transform shootingPoint;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange, playerObjectInSightRange, playerObjectInAttackRange, playerOutpostInSightRange, playerOutpostInAttackRange;

    //Enemy Animator
    private Animator enemyAnim;

    private void Awake()
    {
        //Finding Player by its name in scene (Subject to change)
        agent = GetComponent<NavMeshAgent>();
        enemyAnim = GetComponent<Animator>();
    }

    private void Update()
    {
        //Check for sight and attack range of player
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer) && player!=null;

        //Check for sight and attack range of Object
        playerObjectInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsObject);
        playerObjectInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsObject) && Object!=null;

        //Check for attack range of Outpost
        playerOutpostInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayerOutpost);
        playerOutpostInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayerOutpost);

        //Player State Checks
        if (!playerInSightRange && !playerInAttackRange && !LevelManager.instance.AttackPhase && !playerObjectInAttackRange && !playerObjectInSightRange) Patroling();
        if ((playerInSightRange && !playerInAttackRange) || (playerObjectInSightRange && !playerObjectInAttackRange)) ChasePlayer();
        if ((playerInAttackRange && playerInSightRange) || (playerObjectInAttackRange && playerObjectInSightRange) || (playerOutpostInAttackRange && playerOutpostInSightRange)) AttackPlayer();
        if (!playerInSightRange && !playerInAttackRange && !playerObjectInSightRange && !playerObjectInAttackRange && !playerOutpostInAttackRange && LevelManager.instance.AttackPhase) GotoOutpost();
    }

    //Enemy State Functions
    private void Patroling()
    {
        //Enemy Walking Animation
        enemyAnim.SetBool("isShooting", false);
        enemyAnim.SetBool("isWalking", true);

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        //Enemy Walking Animation
        enemyAnim.SetBool("isShooting", false);
        enemyAnim.SetBool("isWalking", true);

        Collider[] rangeChecksPlayer = Physics.OverlapSphere(transform.position, sightRange, whatIsPlayer);
        Collider[] rangeChecksObject = Physics.OverlapSphere(transform.position, sightRange, whatIsObject);
        Collider[] rangeChecksOutpost = Physics.OverlapSphere(transform.position, sightRange, whatIsPlayerOutpost);

        if (rangeChecksPlayer.Length != 0)
        {
            foreach (Collider c in rangeChecksPlayer)
            {
                if (c != null)
                {
                    player = rangeChecksPlayer[0].transform;
                    agent.SetDestination(player.position);
                    break;
                }
                else
                {
                    continue;
                }
            }    
        }
        else if (rangeChecksObject.Length !=0)
        {
            foreach (Collider c in rangeChecksObject)
            {
                if (c != null)
                {
                    Object = rangeChecksObject[0].transform;
                    agent.SetDestination(Object.position);
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
                    Outpost = rangeChecksOutpost[0].transform;
                    agent.SetDestination(Outpost.position);
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
        if (player!=null && playerInAttackRange)
        {    
            transform.LookAt(player);
        }
        else if(Object!=null && playerObjectInAttackRange)
        {
            transform.LookAt(Object);
        }
        else if (Outpost != null && playerOutpostInAttackRange)
        {
            transform.LookAt(Outpost);
        }
        else
        {
            //Do Nothing
        }

        if (!alreadyAttacked)
        {
            ///Attack code here
            //Enemy Shooting Animation
            enemyAnim.SetBool("isShooting", true);
            enemyAnim.SetBool("isWalking", false);

            //Enemy Shooting Sound

            Rigidbody rb = Instantiate(projectile, shootingPoint.position, Quaternion.identity).GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            //rb.AddForce(transform.up * 8f, ForceMode.Impulse);
            ///End of attack code

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    public void GotoOutpost()
    {
        //Enemy will goto Player's Outpost Position
        if (OutpostPosition !=null)
        {
            //Enemy Walking Animation
            enemyAnim.SetBool("isShooting", false);
            enemyAnim.SetBool("isWalking", true);

            agent.SetDestination(OutpostPosition.position);
        }
        else
        {
            //Enemy Victory Animation
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
