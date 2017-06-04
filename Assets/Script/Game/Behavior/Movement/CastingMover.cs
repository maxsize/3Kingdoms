using Assets.Script.Game.Data;
using ThreeK.Game.Behavior.Movement;
using UnityEngine;

namespace ThreeK.Game.Behavior.Movement
{
    public class CastingMover : Mover
    {
        private Vector3 _target;

        protected override void SetTarget(Vector3 target, float latency)
        {
            _target = target;
            base.SetTarget(target, latency);
        }

        protected override bool IsReached()
        {
            var ability = Data.Ability;
            var radius = ability.Radius;
            var dist = Vector3.Distance(transform.position, _target);
            return dist <= radius;
        }

        protected override void FixedUpdate()
        {
            if (_target != null)
                transform.rotation = Quaternion.LookRotation(_target - transform.position);
            base.FixedUpdate();
        }
    }
}
