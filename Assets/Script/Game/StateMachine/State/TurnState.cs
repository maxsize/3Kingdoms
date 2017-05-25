using UnityEngine;
using System.Collections;
using System;
using ThreeK.Game.StateMachine.Input;
using ThreeK.Game.StateMachine.State;
using Adic;
using Adic.Container;
using ThreeK.Game.StateMachine;
using ThreeK.Game.Helper;

namespace ThreeK.Game.StateMachine.State
{
    public class TurnState : State
    {
        [Inject] public IInjectionContainer Container;

        private Vector3 _target;
        private Quaternion _data;

        public TurnState(IStateMachine stateMachine)
        {
            _target = GetTarget(InputHelper.CurrentInput);
            var trans = (stateMachine as MonoBehaviour).transform;
            _data = Quaternion.LookRotation(_target - trans.position);
        }

        public override object Data
        {
            get { return _data; }
        }

        public override IState HandleInput(IInput input)
        {
            if (input is AttackInput || input is MoveInput || input is CastInput)
            {
                return Container.Resolve<IState>(typeof(StackedState));
            }
            return this;
        }

        private Vector3 GetTarget(IInput input)
        {
            if (input.Data is Vector3)
                return (Vector3) input.Data;
            if (input.Data is Transform)
                return ((Transform) input.Data).position;
            return Vector3.zero;
        }
    }
}
