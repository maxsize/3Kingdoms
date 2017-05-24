using UnityEngine;
using System.Collections;
using ThreeK.Game.Behavior.Core;

namespace ThreeK.Game.Behavior.Movement
{
    public class Stander : MovementBehaviour
    {
        protected Vector3 Position = Vector3.zero;

        private void FixedUpdate()
        {
            if (Position != Vector3.zero)
                transform.position = Position;
        }

        private void OnDisable()
        {
            Position = Vector3.zero;
        }

        public override void End()
        {
            enabled = false;
        }

        protected override void SetTarget()
        {
            var animator = GetComponent<Animator>();
            animator.SetBool("Moving", false);
            animator.SetBool("Running", false);
            Position = transform.position;
        }
    }
}
