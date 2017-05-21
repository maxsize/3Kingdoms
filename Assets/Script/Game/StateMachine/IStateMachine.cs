using UnityEngine;
using System.Collections.Generic;
using ThreeK.Game.StateMachine.State;
using ThreeK.Game.StateMachine.Input;
using UnityEngine.Events;
using System;

namespace ThreeK.Game.StateMachine
{
    public interface IStateMachine
    {
        StateChangeEvent OnStateChange { get; }
        IState CurrentState { get; }
        void AddState(IState defaultState);
        /// <summary>
        /// Handle input, generate and return next state
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        IState HandleInput(IInput input);
    }

    public class StateChangeEvent : UnityEvent<IState>
    {
        internal void AddListener()
        {
            throw new NotImplementedException();
        }

        internal void AddListener(StateChangeEvent onStateChange)
        {
            throw new NotImplementedException();
        }
    }
}
