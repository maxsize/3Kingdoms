using Adic;
using System.Collections.Generic;
using ThreeK.Game.StateMachine.Input;
using ThreeK.Game.StateMachine.State;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace ThreeK.Game.StateMachine
{
    public class StateMachine : MonoBehaviour, IStateMachine
    {
        public IState CurrentState { get; protected set; }

        public StateChangeEvent OnStateChange {
            get { return _onStateChange; }
        }

        private readonly StateChangeEvent _onStateChange = new StateChangeEvent();
        private List<IState> _states = new List<IState>();

        public virtual void AddStates(IState[] states, IState defaultState)
        {
            _states.AddRange(states);
            ChangeState(defaultState);
        }

        public IEnumerable<IState> GetStates()
        {
            return _states.ToArray();
        }

        public virtual IState HandleInput(IInput input)
        {
            var next = CurrentState.HandleInput(input);
            ChangeState(next);
            return next;
        }

        protected virtual void ChangeState(IState newState)
        {
            CurrentState = newState;
            OnStateChange.Invoke(CurrentState);
        }
    }
}
