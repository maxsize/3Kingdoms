using Adic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : MonoBehaviour {

    [Inject] public Player PlayerObj;

    private Vector3 offset;
    private Vector3 cache;

	// Use this for initialization
	void Start () {
        this.Inject();
        cache = PlayerObj.transform.position;
	}

    private void LateUpdate()
    {
        offset = PlayerObj.transform.position - cache;
        transform.position += offset;
        cache = PlayerObj.transform.position;
    }
}
