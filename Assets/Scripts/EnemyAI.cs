using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(EnemySight))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(EnemyExtendedAI))]
public class EnemyAI : MonoBehaviour {

    enum AIState
    {
        PATROL,
        CHECK,
        CHASE,
        EXTENDED
    }

    [SerializeField]
    List<Transform> targetPositions;

    GameObject player;
    EnemySight sight;
    NavMeshAgent agent;
    AIState aiState;
    Vector3 currentTarget;
    EnemyExtendedAI extendedAI;
    bool changed;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        targetPositions.Add(transform);
        agent = GetComponent<NavMeshAgent>();
        sight = GetComponent<EnemySight>();
        currentTarget = transform.position;
        extendedAI = GetComponent<EnemyExtendedAI>();
        changed = false;
        aiState = AIState.PATROL;
    }

    private void Start()
    {
        StartCoroutine(Patrol());
    }

    private void Update()
    {
        if(extendedAI.bState != EnemyExtendedAI.BehaviourState.None)
        {
            aiState = AIState.EXTENDED;
            changed = true;
        }
        else if(changed)
        {
            aiState = AIState.PATROL;
            changed = false;
        }

        if(sight.playerInSight)
        {
            aiState = AIState.CHASE;
        }
        else
        {
            aiState = AIState.PATROL;
        }

        if (sight.playerIsHeard && !sight.playerInSight)
        {
            aiState = AIState.CHECK;
        }
        else if(!sight.playerInSight)
        {
            aiState = AIState.PATROL;
        }
    }

    IEnumerator Patrol()
    {
        while (aiState == AIState.PATROL)
        {
            Debug.Log( gameObject.name + ": I'm on patrol");
            yield return new WaitForSeconds(0.2f);
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
            yield return new WaitForSeconds(0.2f);
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
            yield return new WaitForSeconds(0.2f);
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

}