using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTriggerController : MonoBehaviour {

    [SerializeField]
    BulletType bulletType;
    int bulletCount;

    private void Start()
    {
        bulletCount = 1;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if (Input.GetKeyDown(KeyCode.E) && bulletCount > 0)
            {
                other.gameObject.GetComponent<ShootingScript>().bulletList.Find(bullet => bullet.type == bulletType).PickupBullet();
                bulletCount--;
            }
        }
    }
}
