using Adic;
using System;
using System.Collections.Generic;
using ThreeK.Game.StateMachine.Input;
using ThreeK.Game.StateMachine.State;
using UnityEngine;
using Adic.Injection;
using Adic.Container;

namespace ThreeK.Game.StateMachine
{
    public class StateMachine : IMonoStateMachine
    {
        private IState _currentState;
        private List<IState> _states;
        private MonoBehaviour _client;

        public StateMachine(MonoBehaviour client)
        {
            _states = new List<IState>();
            _client = client;
        }

        public IState CurrentState
        {
            get { return _currentState; }
        }

        public MonoBehaviour Client
        {
            get { return _client; }
        }

        public void AddStates(IState[] states, IState defaultState)
        {
            _states.AddRange(states);
            if (!_states.Contains(defaultState))
                _states.Add(defaultState);
            _currentState = defaultState;
        }

        public IEnumerable<IState> GetStates()
        {
            return _states.ToArray();
        }

        public void HandleInput(IInput input)
        {
            var next = _currentState.HandleInput(input);
            next.Enter(input);
            _currentState = next;
        }

        protected void ChangeState(IState state)
        {

        }

        public class Factory : IFactory
        {
            [Inject] public IInjectionContainer Container;

            public object Create(InjectionContext context)
            {
                var machine = new StateMachine(context.parentInstance as MonoBehaviour);
                List<IState> states = new List<IState>
                {
                    new IdleState(machine),
                    new AttackState(machine),
                    new MoveState(machine)
                };
                machine.AddStates(states.ToArray(), states[0]);
                return machine;
            }
        }
    }
}
