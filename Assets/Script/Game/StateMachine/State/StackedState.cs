using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adic;
using Adic.Container;
using ThreeK.Game.StateMachine.Input;
using UnityEngine;
using _3Kingdoms.Game.StateMachine.State;

namespace ThreeK.Game.StateMachine.State
{
    public class StackedState : MonoState, IStateStack
    {
        [Inject]
        public IInjectionContainer Container;

        private List<IState> _stack;
        public StackedState(IStateMachine stateMachine) : base(stateMachine as MonoBehaviour)
        {
            _stack = new List<IState>();
        }

        [Inject]
        public void PostConstruct([Inject("CurrentInput")] IInput input)
        {
            if (input != null)
            {
                _stack.AddRange(CreateStates(input));
            }
        }

        public override IState HandleInput(IInput input)
        {
            return null;
        }

        public IState[] GetStack()
        {
            return _stack.ToArray();
        }

        private List<IState> CreateStates(IInput input)
        {
            var states = new List<IState>();
            if (input is MoveInput)
            {
                // Create turn and move state
                states.Add(Container.Resolve<IState>(typeof(MoveState)));
                states.Add(Container.Resolve<IState>(typeof(TurnState)));
            }
            if (input is AttackInput)
            {
                // Create turn and move state
                states.Add(Container.Resolve<IState>(typeof(AttackState)));
                states.Add(Container.Resolve<IState>(typeof(MoveState)));
                states.Add(Container.Resolve<IState>(typeof(TurnState)));
            }
            return states;
        }
    }
}
