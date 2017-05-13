using System;
using ThreeK.Game.StateMachine.Input;
using UnityEngine.Events;

namespace ThreeK.Game.StateMachine.State
{
    public interface IState
    {
        /// <summary>
        /// Handle input and returns next state.
        /// </summary>
        /// <param name="stateMachine">State machine currently belongs to</param>
        /// <param name="input">Input</param>
        /// <returns></returns>
        IState HandleInput(IInput input);
        object Data { get; }
    }
}

