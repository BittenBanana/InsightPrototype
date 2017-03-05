using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyId : MonoBehaviour {

    string id;

	// Use this for initialization
	void Start () {
        id = Guid.NewGuid().ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
