using UnityEngine;
using System.Collections;

public class FollowTransform : MonoBehaviour {
    [SerializeField]
    Transform target;
    Vector3 offset;

    void Awake()
    {
        offset = transform.position - target.position;
    }

	void Update () {
        transform.position = target.position + offset;
	}
}
