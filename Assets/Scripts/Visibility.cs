using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visibility : MonoBehaviour {

    public enum VisibilityState
    {
        VISIBLE,
        NORMAL
    }
    [SerializeField]
    Material normal;
    [SerializeField]
    Material visible;

    Renderer r;
    VisibilityState vState;

	// Use this for initialization
	void Start () {
        vState = VisibilityState.NORMAL;
        r = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.V)) {
            if(vState == VisibilityState.NORMAL)
            {
                r.material = visible;
                vState = VisibilityState.VISIBLE;
            }
            else
            {
                r.material = normal;
                vState = VisibilityState.NORMAL;
            }
        }
	}
}
