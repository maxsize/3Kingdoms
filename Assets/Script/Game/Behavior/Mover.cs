using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    public Vector3 Target;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void FixedUpdate()
    {
        Target.y = 0;
        transform.position = Vector3.MoveTowards(transform.position, Target, 10 * Time.fixedDeltaTime);
        GetComponent<Animator>().SetBool("Moving", true);
        GetComponent<Animator>().SetBool("Running", true);

        if (Vector3.Distance(transform.position, Target) < 0.01)
        {
            GetComponent<Animator>().SetBool("Moving", false);
            GetComponent<Animator>().SetBool("Running", false);
            transform.position = Target;
        }
    }
}
