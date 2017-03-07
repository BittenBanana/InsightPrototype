using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyExtendedAI : MonoBehaviour {
    public enum BehaviourState
    {
        Aggresive,
        Sleep,
        Blind,
        Deaf,
        FollowTransmitter,
        None
    }

    public BehaviourState bState { get; private set; }
    GameObject target = null;
    [SerializeField]
    private float startSleepTime = 5;
    private float sleepTime;
    private float startBlindTime = 5;
    private float blindTime;
    private float startDeafTime = 5;
    private float deafTime;
    // Use this for initialization
    void Start () {
        bState = BehaviourState.None;
        sleepTime = startSleepTime;
        blindTime = startBlindTime;
        deafTime = startDeafTime;
    }
	
	// Update is called once per frame
	void Update () {
        switch(bState)
        {
            case BehaviourState.None:
                break;
            case BehaviourState.Aggresive:
                {
                    this.gameObject.GetComponent<NavMeshAgent>().SetDestination(target.gameObject.transform.position);
                    //this.transform.position = Vector3.MoveTowards(this.transform.position, target.gameObject.transform.position, .1f);
                    Debug.Log(Vector3.Distance(this.transform.position, target.transform.position));
                    if(Vector3.Distance(this.transform.position, target.transform.position) < 3.0f)
                    {
                        this.gameObject.GetComponent<NavMeshAgent>().ResetPath();
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
                        this.gameObject.GetComponent<Renderer>().material.color = Color.red;
                        this.gameObject.GetComponent<NavMeshAgent>().Resume();
                        bState = BehaviourState.None;
                    }
                }
                break;
            case BehaviourState.Blind:
                {
                    blindTime -= Time.deltaTime;
                    Debug.Log(blindTime);

                    if(blindTime <= 0)
                    {
                        blindTime = startBlindTime;
                        this.gameObject.GetComponent<EnemySight>().enemyBlind = false;
                        
                        bState = BehaviourState.None;
                    }
                }
                break;
            case BehaviourState.Deaf:
                {
                    deafTime -= Time.deltaTime;
                    Debug.Log(deafTime);

                    if (deafTime <= 0)
                    {
                        deafTime = startDeafTime;
                        this.gameObject.GetComponent<EnemySight>().enemyDeaf = false;
                        Debug.Log(this.gameObject.GetComponent<EnemySight>().enemyDeaf);
                        bState = BehaviourState.None;
                    }
                }
                break;
            case BehaviourState.FollowTransmitter:
                {
                    if (target != null)
                    {
                        this.gameObject.GetComponent<NavMeshAgent>().SetDestination(target.gameObject.transform.position);
                        if (Vector3.Distance(this.transform.position, target.transform.position) < 3.0f)
                        {
                            this.gameObject.GetComponent<NavMeshAgent>().ResetPath();
                            bState = BehaviourState.None;
                        }
                    }
                    else
                        bState = BehaviourState.None;

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
            case BulletType.Blinding:
                {
                    BlindingBullet();
                }
                break;
            case BulletType.Deafening:
                {
                    DeafeningBullet();
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
        if(bState == BehaviourState.None)
        {
            Debug.Log("Im agresive");
            target = FindNearestEnemy(100).gameObject;
            bState = BehaviourState.Aggresive;
        }
    }

    private void SleepBullet()
    {
        if(bState == BehaviourState.None)
        {
            this.gameObject.GetComponent<NavMeshAgent>().Stop();//!!!
            this.gameObject.GetComponent<Renderer>().material.color = Color.blue;
            Debug.Log("I'm asleep----------------------------");
            bState = BehaviourState.Sleep;
        }
    }

    private void BlindingBullet()
    {
        if(bState == BehaviourState.None)
        {
            this.gameObject.GetComponent<EnemySight>().enemyBlind = true;
            Debug.Log("I'm blinded");
            bState = BehaviourState.Blind;
        }
    }

    private void DeafeningBullet()
    {
        if (bState == BehaviourState.None)
        {
            this.gameObject.GetComponent<EnemySight>().enemyDeaf = true;
            Debug.Log("I'm deafened");
            bState = BehaviourState.Deaf;
        }
    }

    public void FollowTransmitter(GameObject transmitter)
    {
        if(bState == BehaviourState.None)
        {
            Debug.Log("I'm going to transmitter");
            target = transmitter;
            bState = BehaviourState.FollowTransmitter;
        }
    }

}


