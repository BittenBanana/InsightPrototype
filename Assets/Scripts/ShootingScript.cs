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
    /// <summary>
    /// tmp
    /// </summary>
    bool vfxBulletState = false;
    GameObject vfxButtet;

    LineRenderer line;
    // Use this for initialization
    void Start () {
        state = ShootingState.Free;
        currentBullet = BulletType.None;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        line = GetComponent<LineRenderer>();
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;
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
        if(Input.GetKeyDown(KeyCode.Alpha5))
        {
            currentBullet = BulletType.WallTransmitter;
            Debug.Log("Relodaded to wall focus");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            currentBullet = BulletType.WallVisibility;
            Debug.Log("Relodaded to wall visibility");
        }
        if(Input.GetKeyDown(KeyCode.Alpha0))
        {
            currentBullet = BulletType.VFX;
            Debug.Log("RAINBOOOOOOW");
        }

        if(vfxBulletState)
        {
            vfxButtet.transform.position = Vector3.MoveTowards(vfxButtet.transform.position, shootHit.point, 0.5f);
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
        Ray cameraRay = new Ray(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2)), Camera.main.transform.forward * 50.0f); // Camera.main.ScreenToWorldPoint, Camera.main.transform.forward
        if (Physics.Raycast(cameraRay, out shootHit))
        {
            if (currentBullet != BulletType.VFX)
            {
                if (shootHit.collider.tag == "Enemy")
                {
                    shootHit.collider.GetComponent<EnemyExtendedAI>().Inject(currentBullet);
                }
                if (shootHit.collider.tag == "Wall")
                {
                    if (currentBullet == BulletType.WallTransmitter)
                    {
                        Instantiate(Resources.Load("WallTransmitter"), shootHit.point, Quaternion.identity);
                    }
                    if (currentBullet == BulletType.WallVisibility)
                    {
                        Instantiate(Resources.Load("WallSight"), shootHit.point, Quaternion.identity);
                    }

                }
            }
            else
            {
                vfxButtet = (GameObject)Instantiate(Resources.Load("ParticleVFX"), Hand.transform.position, Quaternion.identity);
                vfxBulletState = true;
            }
        }
        //DrawLine(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2)), Camera.main.transform.forward * 50.0f);
        //Debug.DrawRay(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2)), Camera.main.transform.forward * 50.0f, Color.red, 22.2f);
        state = ShootingState.Free;
    }

    private void DrawLine(Vector3 vect, Vector3 vect2)
    {
        line.SetPosition(0, vect);
        line.SetPosition(1, vect2);
    }
}
