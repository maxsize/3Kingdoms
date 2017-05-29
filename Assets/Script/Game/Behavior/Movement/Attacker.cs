using UnityEngine;
using System.Collections;
using ThreeK.Game.Behavior.Core;

namespace ThreeK.Game.Behavior.Movement
{
    public class Attacker : Stander
    {
        private const string ATTACK1_TRIGGER = "Attack1Trigger";

        protected override void SetTarget(Transform target, float latency)
        {
            SetTarget(latency);
            var animator = GetComponent<Animator>();
            animator.SetTrigger(ATTACK1_TRIGGER);
            StartCoroutine(Wait(1.2f));
        }

        public override void End()
        {
        }

        private IEnumerator Wait(float duration)
        {
            yield return new WaitForSeconds(duration);
            OnEnd.Invoke();
            enabled = false;
        }
    }
}
