using UnityEngine;
using System.Collections;
using System;
using Adic;
using ThreeK.Game.StateMachine.Input;
using Adic.Container;
using _3Kingdoms.Game.StateMachine.State;

namespace ThreeK.Game.StateMachine.State
{
    [InjectFromContainer("MainContainer")]
    public class IdleState : MonoState
    {
        [Inject] public IInjectionContainer Container;

        public IdleState(IStateMachine stateMachine) : base(stateMachine as MonoBehaviour)
        {
        }

        public override void Enter(IInput input)
        {
            base.Enter(input);

            var animator = Machine.GetComponent<Animator>();
            animator.SetBool("Moving", false);
            animator.SetBool("Running", false);
        }

        public override IState HandleInput(IInput input)
        {
            if (input is AttackInput)
                return Container.Resolve<IState>(typeof(AttackState));
            if (input is MoveInput)
                return Container.Resolve<IState>(typeof(MoveState));
            return this;
        }
    }
}
