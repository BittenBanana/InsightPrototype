using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingScript : MonoBehaviour {

    enum ShootingState
    {
        isShooting,
        Free,
        Reloading
    }
    Bullet currentBullet;
    RaycastHit shootHit;
    ShootingState state;

    public List<Bullet> bulletList { get; private set; }

    public GameObject Hand;
    [SerializeField]
    Text bulletTypeText;

    float seeThroughRenewalTime = 15;
    float seeThroughTimer;
    /// <summary>
    /// tmp
    /// </summary>
    bool vfxBulletState = false;
    GameObject vfxButtet;

    LineRenderer line;
    // Use this for initialization
    void Start() {
        state = ShootingState.Free;
        currentBullet = null;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        line = GetComponent<LineRenderer>();
        line.startWidth = 0.1f;
        line.endWidth = 0.1f;

        bulletList = new List<Bullet>();
        Bullet aggresiveBullet = new Bullet(BulletType.Aggresive);
        aggresiveBullet.OnPickupBullet += new EventHandler(OnPickupBullet);
        aggresiveBullet.OnShoot += new EventHandler(OnShoot);
        bulletList.Add(aggresiveBullet);

        Bullet seeThroughBullet = new Bullet(BulletType.WallVisibility);
        seeThroughBullet.OnPickupBullet += new EventHandler(OnPickupBullet);
        seeThroughBullet.OnShoot += new EventHandler(OnShoot);
        bulletList.Add(seeThroughBullet);

        Bullet transmitterBullet = new Bullet(BulletType.WallTransmitter);
        transmitterBullet.OnPickupBullet += new EventHandler(OnPickupBullet);
        transmitterBullet.OnShoot += new EventHandler(OnShoot);
        bulletList.Add(transmitterBullet);
    }

    void OnPickupBullet(object sender, EventArgs e)
    {
        if(currentBullet != null)
        if(currentBullet.type == (sender  as Bullet).type)
            bulletTypeText.text = currentBullet.type.ToString() + "   " + currentBullet.count;
    }

    void OnShoot(object sender, EventArgs e)
    {
        bulletTypeText.text = currentBullet.type.ToString() + "   " + currentBullet.count;
        if(currentBullet.type == BulletType.WallVisibility)
        {
            seeThroughTimer = seeThroughRenewalTime;
        }
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentBullet = bulletList.Find(bullet => bullet.type == BulletType.Aggresive);
            bulletTypeText.text = currentBullet.type.ToString() + "   " + currentBullet.count;
            Debug.Log("Reloaded to Aggresive");
        }
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    currentBullet = BulletType.Sleep;
        //    Debug.Log("Relodaded to asleep");
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    currentBullet = BulletType.Blinding;
        //    Debug.Log("Relodaded to blinding");
        //}
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    currentBullet = BulletType.Deafening;
        //    Debug.Log("Relodaded to deafening");
        //}
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentBullet = bulletList.Find(bullet => bullet.type == BulletType.WallTransmitter);
            bulletTypeText.text = currentBullet.type.ToString() + "   " + currentBullet.count;
            Debug.Log("Relodaded to wall marker");
        }
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            currentBullet = bulletList.Find(bullet => bullet.type == BulletType.WallVisibility);
            bulletTypeText.text = currentBullet.type.ToString() + "   " + currentBullet.count;
            Debug.Log("Relodaded to wall visibility");
        }
        //if (Input.GetKeyDown(KeyCode.Alpha0))
        //{
        //    currentBullet = BulletType.VFX;
        //    Debug.Log("RAINBOOOOOOW");
        //}

        if (bulletList.Find(b => b.type == BulletType.WallVisibility).count == 0 && seeThroughTimer <= 0)
        {
            bulletList.Find(b => b.type == BulletType.WallVisibility).PickupBullet();
            seeThroughTimer = seeThroughRenewalTime;
        }
        
        if(seeThroughTimer > 0)
        {
            seeThroughTimer -= Time.deltaTime;
        }

        if (vfxBulletState)
        {
            vfxButtet.transform.position = Vector3.MoveTowards(vfxButtet.transform.position, shootHit.point, 0.5f);
        }
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0) && currentBullet.count > 0)
        {
            state = ShootingState.isShooting;
        }
        if (state == ShootingState.isShooting)
        {
            
            Shoot();
        }
    }

    private void Shoot()
    {
        Ray cameraRay = new Ray(Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2)), Camera.main.transform.forward * 50.0f); // Camera.main.ScreenToWorldPoint, Camera.main.transform.forward
        if (Physics.Raycast(cameraRay, out shootHit))
        {
            if (currentBullet.type != BulletType.VFX)
            {
                if (shootHit.collider.tag == "Enemy")
                {
                    shootHit.collider.GetComponent<EnemyExtendedAI>().Inject(currentBullet.type);
                }
                if (shootHit.collider.tag == "Wall")
                {
                    if (currentBullet.type == BulletType.WallTransmitter)
                    {
                        Instantiate(Resources.Load("WallTransmitter"), shootHit.point, Quaternion.identity);
                    }
                    if (currentBullet.type == BulletType.WallVisibility)
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
        currentBullet.Shoot();
        state = ShootingState.Free;
    }

    private void DrawLine(Vector3 vect, Vector3 vect2)
    {
        line.SetPosition(0, vect);
        line.SetPosition(1, vect2);
    }

    public class Bullet
    {
        public BulletType type { get; private set; }
        public int count { get; private set; }
        public event EventHandler OnPickupBullet;
        public event EventHandler OnShoot;

        public Bullet(BulletType type)
        {
            this.type = type;
            count = 0;
        }

        public void Shoot()
        {
            count--;
            OnShoot(this, new EventArgs());
        }

        public void PickupBullet()
        {
            count++;            
            OnPickupBullet(this, new EventArgs());
        }
    }
}
