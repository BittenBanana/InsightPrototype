using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyExtendedAI : MonoBehaviour {
    public enum BehaviourState
    {
        Aggresive,
        Sleep,
        None
    }

    public BehaviourState bState { get; private set; }
    GameObject target = null;
    [SerializeField]
    private float startSleepTime = 5;
    private float sleepTime;
    // Use this for initialization
    void Start () {
        bState = BehaviourState.None;
        sleepTime = startSleepTime;
    }
	
	// Update is called once per frame
	void Update () {
        switch(bState)
        {
            case BehaviourState.None:
                break;
            case BehaviourState.Aggresive:
                {
                    this.transform.position = Vector3.MoveTowards(this.transform.position, target.gameObject.transform.position, .1f);
                    if(Vector3.Distance(this.transform.position, target.transform.position) < 1)
                    {
                        bState = BehaviourState.None;
                    }
                }
                break;
            case BehaviourState.Sleep:
                {          
                    sleepTime -= Time.deltaTime;

                    if(sleepTime <= 0)
                    {
                        sleepTime = startSleepTime;
                        this.gameObject.GetComponent<NavMeshAgent>().Resume();
                        bState = BehaviourState.None;
                    }
                }
                break;
        }
	}

    public void Inject(BulletType type)
    {
        switch(type)
        {
            case BulletType.Aggresive:
                {
                    AggresiveBullet();
                }
                break;
            case BulletType.Sleep:
                {
                    SleepBullet();
                }
                break;
        }
    }

    private Collider FindNearestEnemy(float radius)
    {
        Collider nearest = new Collider();
        float nearestDistance = 0;
        Collider[] hitColliders = Physics.OverlapSphere(this.transform.position, radius);
        int i = 0;
        foreach (var collider in hitColliders)
        {
            if(collider.tag == "Enemy")
            {
                if (nearest == null && collider.gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
                {
                    nearest = collider;
                    nearestDistance = Vector3.Distance(this.transform.position, nearest.transform.position);
                }
                else
                {
                    if(Vector3.Distance(this.transform.position, collider.transform.position) < nearestDistance 
                        && collider.gameObject.GetInstanceID() != this.gameObject.GetInstanceID())
                    {
                        nearest = collider;
                        nearestDistance = Vector3.Distance(this.transform.position, nearest.transform.position);
                    }
                }
            }
        }
        return nearest;
    }

    private void AggresiveBullet()
    {
        if(bState != BehaviourState.Aggresive)
        {
            Debug.Log("Im agresive");
            target = FindNearestEnemy(100).gameObject;
            bState = BehaviourState.Aggresive;
        }
    }

    private void SleepBullet()
    {
        if(bState != BehaviourState.Sleep)
        {
            this.gameObject.GetComponent<NavMeshAgent>().Stop();
            Debug.Log("I'm asleep----------------------------");
            bState = BehaviourState.Sleep;
        }
    }
}


