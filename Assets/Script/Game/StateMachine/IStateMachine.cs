using UnityEngine;
using System.Collections.Generic;
using ThreeK.Game.StateMachine.State;
using ThreeK.Game.StateMachine.Input;
using UnityEngine.Events;

namespace ThreeK.Game.StateMachine
{
    public interface IStateMachine
    {
        StateChangeEvent OnStateChange { get; }
        IEnumerable<IState> GetStates();
        IState CurrentState { get; }
        void AddStates(IState[] states, IState defaultState);
        /// <summary>
        /// Handle input, generate and return next state
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        IState HandleInput(IInput input);
    }

    public class StateChangeEvent : UnityEvent<IState>
    { }
}
