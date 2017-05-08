using UnityEngine;
using System.Collections;
using System;
using Adic;
using ThreeK.Game.StateMachine.Input;
using Adic.Container;

namespace ThreeK.Game.StateMachine.State
{
    public class IdleState : State
    {
        [Inject] public IInjectionContainer Container;

        public IdleState(IMonoStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter(IInput input)
        {
            base.Enter(input);

            var transform = this.stateMachine.Client;
            var animator = transform.GetComponent<Animator>();
            animator.SetBool("Moving", false);
            animator.SetBool("Running", false);
        }

        public override IState HandleInput(IInput input)
        {
            if (input is AttackInput)
                return Container.Resolve<AttackState>();
            if (input is MoveInput)
                return Container.Resolve<MoveState>();
            return this;
        }
    }
}
