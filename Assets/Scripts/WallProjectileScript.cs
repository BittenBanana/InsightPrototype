using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallProjectileScript : MonoBehaviour {
    public float radius;
    float timeElapsed;
	// Use this for initialization
	void Start () {
        timeElapsed = 0;
	}
	
	// Update is called once per frame
	void Update () {

        //Collider[] enemies = Physics.OverlapSphere(this.gameObject.transform.position, radius);

        //foreach (Collider col in enemies)
        //{
        //    if(col.gameObject.tag == "Enemy")
        //    {
        //        Call(col);
        //    }
        //}



        timeElapsed += Time.deltaTime;
        if (timeElapsed > 10f)
            Destroy(this.gameObject);
	}
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Enemy" && this.gameObject != null)
            other.gameObject.GetComponent<EnemyExtendedAI>().FollowTransmitter(this.gameObject);
    }
    private void Call(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && this.gameObject != null)
            other.gameObject.GetComponent<EnemyExtendedAI>().FollowTransmitter(this.gameObject);
    }
}
