using UnityEngine;
using System.Collections;
using System;
using Adic;
using ThreeK.Game.StateMachine.Input;

namespace ThreeK.Game.StateMachine.State
{
    public class AttackState : State
    {
        private const string ATTACK1_TRIGGER = "Attack1Trigger";

        public AttackState(IMonoStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter(IInput input)
        {
            base.Enter(input);

            var transform = this.stateMachine.Client;
            var animator = transform.GetComponent<Animator>();
            animator.SetTrigger(ATTACK1_TRIGGER);
            StartCoroutine(Wait(1.2f));
        }

        public override IState HandleInput(IInput input)
        {
            return this;
        }

        private IEnumerator Wait(float duration)
        {
            yield return new WaitForSeconds(duration);
            Invoke(OnStateExit);
        }
    }
}
