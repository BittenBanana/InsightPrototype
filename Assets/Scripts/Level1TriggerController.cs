using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Level1TriggerController : MonoBehaviour {

    [SerializeField]
    NavMeshAgent enemy;
    [SerializeField]
    Transform targetPoint;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            enemy.SetDestination(targetPoint.position);
        }
    }
}
