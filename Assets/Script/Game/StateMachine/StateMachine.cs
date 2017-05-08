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
    public class StateMachine : InjectableBehaviour, IStateMachine
    {
        public IState CurrentState { get; protected set; }
        private List<IState> _states;

        [Inject]
        public virtual void Construct()
        {
            _states = new List<IState>();
        }

        public virtual void AddStates(IState[] states, IState defaultState)
        {
            _states.AddRange(states);
            if (!_states.Contains(defaultState))
                _states.Add(defaultState);
            CurrentState = defaultState;
        }

        public IEnumerable<IState> GetStates()
        {
            return _states.ToArray();
        }

        public virtual IState HandleInput(IInput input)
        {
            var next = CurrentState.HandleInput(input);
            next.Enter(input);
            CurrentState = next;
            OnStateChange(CurrentState);
            return next;
        }

        protected virtual void OnStateChange(IState newState)
        {
        }
    }
}
