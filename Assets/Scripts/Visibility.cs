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
    VisibilityState vState { get; set; }
    float elapsedTime;
    float visibilityMaxTime = 5;
	// Use this for initialization
	void Start () {
        vState = VisibilityState.NORMAL;
        r = GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        elapsedTime += Time.deltaTime;

        if(elapsedTime >= visibilityMaxTime)
        {
            SetVisible(VisibilityState.NORMAL);
            elapsedTime = 0;
        }
	}

    public void SetVisible(VisibilityState state)
    {
        switch(state)
        {
            case VisibilityState.NORMAL:
                {
                    r.material = normal;
                }
                break;
            case VisibilityState.VISIBLE:
                {
                    r.material = visible;
                }
                break;
        }
    }
}
