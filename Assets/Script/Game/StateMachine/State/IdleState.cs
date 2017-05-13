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

        public IdleState()
        {
        }

        public override object Data
        {
            get { return null; }
        }

        public override IState HandleInput(IInput input)
        {
            if (input is AttackInput)
                return Container.Resolve<IState>(typeof(StackedState));
            if (input is MoveInput)
                return Container.Resolve<IState>(typeof(StackedState));
            return this;
        }
    }
}
