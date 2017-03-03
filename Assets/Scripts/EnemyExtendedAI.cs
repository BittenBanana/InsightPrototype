using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyExtendedAI : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void Inject(BulletType type)
    {
        switch(type)
        {
            case BulletType.Aggresive:
                {
                    Debug.Log("Oh no im agresive");
                }
                break;
            case BulletType.Sleep:
                {
                    Debug.Log("Oh no im asleep");
                }
                break;
        }
    }
}
