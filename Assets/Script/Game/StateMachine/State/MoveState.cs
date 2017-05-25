using UnityEngine;
using System.Collections;
using System;
using Adic;
using ThreeK.Game.StateMachine.Input;
using Adic.Container;
using ThreeK.Game.Helper;

namespace ThreeK.Game.StateMachine.State
{
    public class MoveState : State
    {
        [Inject] public IInjectionContainer Container;
        
        private Vector3 _target;

        public MoveState()
        {
            _target = GetDestination(InputHelper.CurrentInput);
        }

        public override object Data
        {
            get { return _target; }
        }

        public override IState HandleInput(IInput input)
        {
            if (input is AttackInput || input is MoveInput || input is CastInput)
            {
                return Container.Resolve<IState>(typeof(StackedState));
            }
            return Container.Resolve<IState>(typeof(IdleState));
        }

        protected virtual Vector3 GetDestination(IInput input)
        {
            return (Vector3) input.Data;
        }
    }
}
