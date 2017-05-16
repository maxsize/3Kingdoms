using System;
using System.Collections;
using System.Collections.Generic;
using ThreeK.Game.Behavior.Core;
using UnityEngine;

namespace ThreeK.Game.Behavior.Movement
{
    public class Spinner : MovementBehaviour {

        public float TurnSpeed = 400f;

        private Quaternion _targetQuaternion;

        /// <summary>
        /// Set target and start to turn to the target
        /// </summary>
        /// <param name="target"></param>
        protected override void SetTarget(Quaternion target, float latency)
        {
            _targetQuaternion = target;
            _targetQuaternion.x = _targetQuaternion.z = 0;
            var animator = GetComponent<Animator>();
            animator.SetBool("Moving", false);
            animator.SetBool("Running", false);
            TurnToTarget(latency);  // delay compensation
        }

        private void FixedUpdate()
        {
            TurnToTarget(Time.fixedDeltaTime);
        }

        private void TurnToTarget(float delta)
        {
            var r = Quaternion.RotateTowards(transform.rotation, _targetQuaternion, TurnSpeed * delta);
            transform.rotation = r;
            var diff = Mathf.Abs(transform.eulerAngles.y - _targetQuaternion.eulerAngles.y);
            //Debug.Log(string.Format("{0}, {1}, {2}", diff, trans.rotation.eulerAngles.y, q.eulerAngles.y));
            if (diff < 1)
            {
                transform.rotation = _targetQuaternion;
                OnEnd.Invoke();
                enabled = false;
            }
        }
    }
}
