using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    private int health;
    [SerializeField]
    Image image;
	
	void Start () {
        health = 3;
	}
	
	void Update () {
        switch (health)
        {
            case 0:
                image.color = new Color(image.color.r, image.color.g, image.color.b, 1);
                break;
            case 1:
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.66f);
                break;
            case 2:
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.33f);
                break;
            default:
                break;
        }
	}

    public void playerHit()
    {
        health--;
    }
}
