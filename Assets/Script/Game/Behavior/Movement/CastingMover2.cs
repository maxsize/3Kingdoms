using Assets.Script.Game.Data;
using ThreeK.Game.Behavior.Movement;
using UnityEngine;

namespace ThreeK.Game.Behavior.Movement
{
    public class CastingMover2 : Mover2
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
            var ability = Data.Ability;
            var radius = ability.Radius;
            var dist = Vector3.Distance(transform.position, _target.position);
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
