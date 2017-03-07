using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour {

    enum ShootingState
    {
        isShooting,
        Free,
        Reloading
    }
    BulletType currentBullet;
    RaycastHit shootHit;
    ShootingState state;
    public GameObject Hand;
	// Use this for initialization
	void Start () {
        state = ShootingState.Free;
        currentBullet = BulletType.None;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentBullet = BulletType.Aggresive;
            Debug.Log("Reloaded to Aggresive");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentBullet = BulletType.Sleep;
            Debug.Log("Relodaded to asleep");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentBullet = BulletType.Blinding;
            Debug.Log("Relodaded to blinding");
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            currentBullet = BulletType.Deafening;
            Debug.Log("Relodaded to deafening");
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            state = ShootingState.isShooting;
        }
        if (state == ShootingState.isShooting)
            Shoot();
    }

    private void Shoot()
    {
        //Ray shootRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

        //if (Physics.Raycast(shootRay.origin, shootRay.direction, out shootHit, 10))
        //{
        //    DrawLine(new Vector3(Screen.width/2, Screen.height / 2), shootHit.point);
        //    if (shootHit.collider.tag == "Enemy")
        //    {
        //        shootHit.collider.GetComponent<EnemyExtendedAI>().Inject(currentBullet);
        //    }
        //}
        //else
        //{
        //    DrawLine(new Vector3(Screen.width / 2, Screen.height / 2), shootRay.origin + shootRay.direction * 10);
        //}
        //Debug.DrawRay(shootRay.origin, shootRay.direction * 10, Color.red, 2);
        //state = ShootingState.Free;
        Ray cameraRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if(Physics.Raycast(cameraRay, out shootHit))
        {
            DrawLine(Hand.transform.position, shootHit.point);
            if (shootHit.collider.tag == "Enemy")
            {
                shootHit.collider.GetComponent<EnemyExtendedAI>().Inject(currentBullet);
            }
        }
        state = ShootingState.Free;
    }

    private void DrawLine(Vector3 vect, Vector3 vect2)
    {
        LineRenderer line = GetComponent<LineRenderer>();
        line.SetPosition(0, vect);
        line.SetPosition(1, vect2);
    }
}
