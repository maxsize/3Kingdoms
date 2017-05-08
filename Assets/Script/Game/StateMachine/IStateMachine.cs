using UnityEngine;
using System.Collections.Generic;
using ThreeK.Game.StateMachine.State;
using ThreeK.Game.StateMachine.Input;

namespace ThreeK.Game.StateMachine
{
    public interface IStateMachine
    {
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
}
