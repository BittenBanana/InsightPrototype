using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemySight))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyExtendedAI))]
public class EnemyAI : MonoBehaviour {

    public enum AIState
    {
        PATROL,
        CHECK,
        CHASE,
        EXTENDED
    }

    [SerializeField, Range(0, 10)]
    float standPatrolTime;
    [SerializeField, Range(0, 10)]
    float standCheckTime;
    [SerializeField, Range(0, 10)]
    float standChaseTime;
    [SerializeField, Range(0.25f, 0.74f)]
    float fireRate;
    [SerializeField, Tooltip("At least 2 or freeze")]
    Transform[] targetPositions;

    GameObject player;
    EnemySight sight;
    NavMeshAgent agent;
    public AIState aiState { get; private set; }
    Vector3 currentTarget;
    EnemyExtendedAI extendedAI;
    bool changed;
    Vector3 lastKnownLocation;
    Vector3 lastKnownPlayerLocation;

    float patrolTimer;
    float checkTimer;
    float fireTimer;
    float chaseTimer;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        sight = GetComponent<EnemySight>();
        lastKnownLocation = transform.position;
        extendedAI = GetComponent<EnemyExtendedAI>();
        changed = false;
        aiState = AIState.PATROL;
    }

    private void Start()
    {
        StartCoroutine(Patrol());
        patrolTimer = 0;
        checkTimer = standCheckTime;
        chaseTimer = standChaseTime;
        fireTimer = 0;
    }

    private void Update()
    {
        if(extendedAI.bState != EnemyExtendedAI.BehaviourState.None)
        {
            aiState = AIState.EXTENDED;
            StopAllCoroutines();
            changed = true;
        }
        else if (changed)
        {
            aiState = AIState.PATROL;
            StartCoroutine(Patrol());
            changed = false;
        }

        if (sight.playerInSight)
        {
            aiState = AIState.CHASE;
            lastKnownPlayerLocation = player.transform.position;
        }

        if (sight.playerIsHeard && !sight.playerInSight)
        {
            aiState = AIState.CHECK;
            lastKnownPlayerLocation = player.transform.position;
        }

        Debug.Log(aiState);

    }

    IEnumerator Patrol()
    {
        while (aiState == AIState.PATROL)
        {
            Debug.Log( gameObject.name + ": I'm on patrol");
            if(agent.remainingDistance < agent.stoppingDistance)
            {
                if (patrolTimer <= 0)
                {
                    SetTarget();
                    agent.SetDestination(currentTarget);
                    patrolTimer = standPatrolTime;
                }
                patrolTimer -= Time.deltaTime;
            }
            yield return null;
        }

        if(aiState == AIState.CHASE)
        {
            StartCoroutine(Chase());
        }

        if(aiState == AIState.CHECK)
        {
            StartCoroutine(Check());
        }

        yield return null;
    }

    IEnumerator Chase()
    {
        while (aiState == AIState.CHASE)
        {
            Debug.Log(gameObject.name + ": I'm chasing player");
            agent.SetDestination(SetChaseTarget());
            if (sight.playerInSight)
            {
                Shoot();
            }
            if (agent.remainingDistance < agent.stoppingDistance)
            {
                if(chaseTimer <= 0)
                {
                    aiState = AIState.PATROL;
                    chaseTimer = standChaseTime;
                }
                chaseTimer -= Time.deltaTime;             
            }
            yield return null;
        }

        if (aiState == AIState.PATROL)
        {
            StartCoroutine(Patrol());
        }

        if (aiState == AIState.CHECK)
        {
            StartCoroutine(Check());
        }

        yield return null;
    }

    IEnumerator Check()
    {
        while (aiState == AIState.CHECK)
        {
            Debug.Log(gameObject.name + ": I heard something there ");
            agent.SetDestination(lastKnownPlayerLocation);
            if(agent.remainingDistance < agent.stoppingDistance)
            {
                if(checkTimer <= 0)
                {
                    aiState = AIState.PATROL;
                    checkTimer = standCheckTime;
                }
                checkTimer -= Time.deltaTime;
            }
            yield return null;
        }

        if (aiState == AIState.CHASE)
        {
            StartCoroutine(Chase());
        }

        if (aiState == AIState.PATROL)
        {
            StartCoroutine(Patrol());
        }

        yield return null;
    }

    void SetTarget()
    {
        if (targetPositions.Length >= 2)
        {
            do
            {
                currentTarget = targetPositions[Random.Range(0, targetPositions.Length)].position;
            }
            while (currentTarget == lastKnownLocation);
        }
        else if(targetPositions.Length == 1)
        {
            currentTarget = targetPositions[0].position;
        }
        else
            currentTarget = transform.position;
        lastKnownLocation = currentTarget;
    }

    Vector3 SetChaseTarget()
    {
        Vector3 direction = lastKnownPlayerLocation - transform.position;
        Vector3 destination = lastKnownPlayerLocation - (direction.normalized * 4);
        return destination;
    }

    void Shoot()
    {
        if (fireTimer <= 0)
        {
            RaycastHit hit;

            Vector3 direction = player.transform.position - transform.position + Vector3.up;
            direction += new Vector3(Random.Range(-0.25f, 0.25f) * agent.velocity.magnitude, Random.Range(-0.25f, 0.25f) * agent.velocity.magnitude, Random.Range(-0.25f, 0.25f)* agent.velocity.magnitude);

            Debug.DrawRay(transform.position, direction * 100, Color.red, 0.5f);

            if (Physics.Raycast(transform.position, direction.normalized, out hit, 100f))
            {
                if (hit.collider.gameObject == player)
                {
                    Debug.Log("Player Got Shoot");
                    player.GetComponent<PlayerController>().playerHit();
                }
            }
            fireTimer = fireRate;
        }
        fireTimer -= Time.deltaTime;
    }

}