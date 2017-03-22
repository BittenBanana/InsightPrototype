using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

    private int health;

	void Start () {
        health = 3;
	}
	
	// Update is called once per frame
	void Update () {
		if(health <= 0)
        {
            gameObject.SetActive(false);
        }
	}

    public void enemyHit()
    {
        health--;
    }
}
