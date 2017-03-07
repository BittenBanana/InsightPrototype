using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemySight : MonoBehaviour {

    [SerializeField]
    float fieldOfViewAngle = 110f;
    public bool playerInSight { get; private set; }
    public bool playerIsHeard { get; private set; }
    public bool enemyBlind { get; set; }
    public bool enemyDeaf { get; set; }
    public Vector3 personalLastSighting { get; private set; }

    private NavMeshAgent nav;
    private SphereCollider col;
    private GameObject player;
    private Vector3 previousSighting;

    void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
        col = GetComponentInChildren<SphereCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject == player)
        {
            playerInSight = false;
            playerIsHeard = false;

            Vector3 direction = other.transform.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);

           
            
            if (angle < fieldOfViewAngle * 0.5f)
            {
                RaycastHit hit;

                if (Physics.Raycast(transform.position, direction.normalized, out hit, col.radius))
                {
                    if (hit.collider.gameObject == player)
                    {
                    if (enemyBlind == false)
                            playerInSight = true;
                        //Debug.Log("Player in sight");
                    }
                }
            }
            
            

            
            
            if (CalculatePathLength(player.transform.position) <= col.radius)
            {
                Debug.Log(enemyDeaf + "--------------------------------------------------------");
                if (enemyDeaf == false)
                    playerIsHeard = true;
                if (player.GetComponent<ThirdPersonCharacter>().mState == ThirdPersonCharacter.MoveState.STAND || player.GetComponent<ThirdPersonCharacter>().mState == ThirdPersonCharacter.MoveState.CROUCH)
                {
                    playerIsHeard = false;
                }
                //Debug.Log("Player heard");
            }
            
                    
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerIsHeard = false;
        playerInSight = false;
    }

    float CalculatePathLength(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();

        if(nav.enabled)
        {
            nav.CalculatePath(targetPosition, path);
        }

        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

        allWayPoints[0] = transform.position;
        allWayPoints[allWayPoints.Length - 1] = targetPosition;

        for(int i = 0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        float pathLength = 0;

        for(int i = 0; i < allWayPoints.Length - 1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength;
    }
}
