using Assets.Script.Game.Data;
using ThreeK.Game.Behavior.Movement;
using UnityEngine;

namespace ThreeK.Game.Behavior.Movement
{
    public class CastingMover : Mover
    {
        private Transform _target;

        protected override void SetTarget(Transform target, float latency)
        {
            SetTarget(target.position, latency);
            _target = target;
            StartMovement();
        }

        protected override bool IsReached()
        {
            var ability = (AbilityVO) Data;
            var radius = ability.Radius;
            var dist = Vector3.Distance(transform.position, _target.position);
            //Debug.Log(string.Format("{0} {1} {2}", dist, trans.position, _target));
            //if (dist < 1)
            //{
            //    var animator = GetComponent<Animator>();
            //    animator.SetBool("Moving", false);
            //    animator.SetBool("Running", false);
            //}
            return dist <= radius;
        }

        protected override void FixedUpdate()
        {
            if (_target != null)
                transform.rotation = Quaternion.LookRotation(_target.position - transform.position);
            base.FixedUpdate();
        }
    }
}
