using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour {

    public float TurnSpeed = 360f;
    public float FULL_TURN_TIME = 0.75f;

    private Vector3 _target = Vector3.zero;
    private float _turningTime = 0;
    private float _maxTurningTime = 0;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var camera = Camera.main;

            var dist = Vector3.Distance(Camera.main.transform.position, transform.position);
            var fwd = camera.transform.TransformDirection(Vector3.forward);
            RaycastHit hit;
            Physics.Raycast(camera.transform.position, fwd, out hit);
            if (hit.point != null)
            {
                dist = Vector3.Distance(camera.transform.position, hit.point);
            }

            _target = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist);
            //_target = new Vector3(277.3f, 122.4f, 65.1f);
            _target = Camera.main.ScreenToWorldPoint(_target);
        }
	}

    private void FixedUpdate()
    {
        TurnToTarget();
    }

    private void TurnToTarget()
    {
        if (_target == Vector3.zero)
            return;

        var q = Quaternion.LookRotation(_target - transform.position);
        if (_turningTime == 0)
        {
            var alpha = Mathf.Abs(q.eulerAngles.y - transform.rotation.eulerAngles.y);
            alpha = alpha > 180 ? 360 - alpha : alpha;
            _maxTurningTime = alpha / 180;
            _maxTurningTime *= FULL_TURN_TIME;
            Debug.Log(string.Format("maxTurning {0}", _maxTurningTime));
            GetComponent<Animator>().SetBool("Moving", false);
            GetComponent<Animator>().SetBool("Running", false);
        }
        var r = Quaternion.RotateTowards(transform.rotation, q, TurnSpeed * Time.fixedDeltaTime);
        r.x = r.z = 0;
        _turningTime += Time.fixedDeltaTime;
        if (_turningTime >= _maxTurningTime)
        {
            MoveTo(_target);
            _target = Vector3.zero;
            q.x = q.z = 0;
            transform.rotation = q;
            _turningTime = 0;
            return;
        }
        transform.rotation = r;
    }

    private void MoveTo(Vector3 target)
    {
        var mover = GetComponent<Mover>();
        mover.Target = target;
    }
}
