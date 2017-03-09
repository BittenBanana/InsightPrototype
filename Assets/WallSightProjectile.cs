using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSightProjectile : MonoBehaviour {

    float timeElapsed;

    // Use this for initialization
    void Start()
    {
        timeElapsed = 0;

        Collider[] enemies = Physics.OverlapSphere(this.gameObject.transform.position, 70.0f);

        foreach (Collider col in enemies)
        {
            if (col.gameObject.tag == "Enemy")
            {
                Call(col);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > 10f)
            Destroy(this.gameObject);
    }

    private void Call(Collider other)
    {
        other.gameObject.GetComponent<Visibility>().SetVisible(Visibility.VisibilityState.VISIBLE);
    }
}
