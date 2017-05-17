using UnityEngine;
using System.Collections;

namespace ThreeK.Game.Behavior.Movement
{
    public class Mover2 : Mover
    {
        private Transform _targetTrans;

        protected override void SetTarget(Transform target, float latency)
        {
            _targetTrans = target;
            transform.Translate(Vector3.forward * latency);
            StartMovement();
        }

        protected override bool IsReached()
        {
            var dist = Vector3.Distance(transform.position, _targetTrans.position);
            //Debug.Log(string.Format("{0} {1} {2}", dist, trans.position, _target));
            if (dist < 1)
            {
                // Close to target, stop running animation
                var animator = GetComponent<Animator>();
                animator.SetBool("Moving", false);
                animator.SetBool("Running", false);
            }
            return dist < Threashold;
        }
    }
}
