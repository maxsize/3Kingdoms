using Adic;
using System.Collections.Generic;
using ThreeK.Game.StateMachine.Input;
using ThreeK.Game.StateMachine.State;
using UnityEngine;
using System;
using UnityEngine.Events;

namespace ThreeK.Game.StateMachine
{
    public class MonoStateMachine : MonoBehaviour, IStateMachine
    {
        public IState CurrentState { get; protected set; }

        public StateChangeEvent OnStateChange {
            get { return _onStateChange; }
        }

        private readonly StateChangeEvent _onStateChange = new StateChangeEvent();

        public virtual void AddState(IState defaultState)
        {
            ChangeState(defaultState);
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
