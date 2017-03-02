using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour {


    [SerializeField]
    private GameObject cameraPivot;

    public Transform a, b, c, d;
    public GameObject player;
    public GameObject playerGun;

    public float pitchMax = 80f;
    public float pitchMin = -80f;

    public float smoothSpeed = 0.3f;

    private Vector3 velocity;
    private Vector3 targetPosition;

    Ray cameraRay;
    RaycastHit cameraHit;

    void Awake()
    {
        targetPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        
    }

    void Update ()
    {
        float angle = 0.0f;
        if (Input.GetAxis("Mouse X") != 0)
        {
            float x = 5 * Input.GetAxis("Mouse X");
            cameraPivot.transform.Rotate(0, x, 0, Space.World);
            player.transform.Rotate(0, x, 0);

            //m_Character.Move(new Vector3(0, x, 0), Input.GetKey(KeyCode.C), CrossPlatformInputManager.GetButtonDown("Jump"));
        }
        if (Input.GetAxis("Mouse Y") != 0)
        {
            float y = 5 * -Input.GetAxis("Mouse Y");
            angle = Vector3.Angle(transform.forward, Vector3.Scale(transform.forward, new Vector3(1, 0, 1)));
            if (transform.forward.y > 0) angle = -angle;

            y = Mathf.Clamp(y, pitchMin - angle, pitchMax - angle);

            if ((angle < pitchMax && Input.GetAxis("Mouse Y") < 0) || (angle > pitchMin && Input.GetAxis("Mouse Y") > 0))
            {
                cameraPivot.transform.Rotate(y, 0, 0, Space.Self);
            }

            //float difference = pitchMax - pitchMin;
            //angle -= pitchMin;
            //float progress = angle / difference;

            //transform.position = cameraPivot.transform.position + Bezier(a.localPosition, b.localPosition, c.localPosition, d.localPosition, progress);
        }
        
        if(Input.GetKeyDown(KeyCode.H))
        {
            Vector3 side = transform.localPosition;
            side.x = -side.x;
            targetPosition = side;

        }

        Aim();
        cameraRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(cameraRay, out cameraHit))
        {
            playerGun.transform.LookAt(cameraHit.point);
        }
        else
        {
            playerGun.transform.LookAt(cameraRay.origin + cameraRay.direction * 100);
        }

        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, smoothSpeed * Time.deltaTime);

    }

    public Vector3 Bezier(Vector3 a, Vector3 b, Vector3 c, Vector3 d, float t)
    {
        float mt = (1 - t);
        float mtmt = mt * mt;
        float tt = t * t;

        return + mt * mtmt * a
             + 3 * mtmt * t * b
             + 3 * mt * tt * c
             + tt * t * d;
    }

    void Aim()
    {
        if (Input.GetMouseButtonDown(1))
        {
            targetPosition = new Vector3(transform.localPosition.x, transform.localPosition.y-0.1f, -0.5f);

        }
        if (Input.GetMouseButtonUp(1))
        {
            targetPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 0.1f, -1.5f);
        }

        
    }
}
