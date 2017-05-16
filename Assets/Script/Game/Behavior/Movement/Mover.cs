using System.Collections;
using System.Collections.Generic;
using ThreeK.Game.Behavior.Core;
using UnityEngine;

namespace ThreeK.Game.Behavior.Movement
{
    public class Mover : MovementBehaviour {

        private Vector3 _target;
        private Rigidbody _rigid;
        private Animator _animator;

        protected override void SetTarget(Vector3 target, float latency)
        {
            _target = target;
            transform.Translate(Vector3.forward * latency);
            StartMovement();
        }

        protected virtual bool IsReached()
        {
            var trans = transform;
            var dist = Vector3.Distance(trans.position, _target);
            //Debug.Log(string.Format("{0} {1} {2}", dist, trans.position, _target));
            return dist < 1;
        }

        protected void StartMovement()
        {
            _rigid = GetComponent<Rigidbody>();
            _rigid.velocity = Vector3.forward * 10;
            _animator = GetComponent<Animator>();
            _animator.SetBool("Moving", true);
            _animator.SetBool("Running", true);
        }

        private void FixedUpdate()
        {
            if (IsReached())
            {
                EndMove();
                enabled = false;
            }
        }

        private void EndMove()
        {
            if (_animator)
            {
                _animator.SetBool("Moving", false);
                _animator.SetBool("Running", false);
                _rigid.velocity = Vector3.zero;
                OnEnd.Invoke();
            }
        }
    }
}
