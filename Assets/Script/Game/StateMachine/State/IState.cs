using System;
using ThreeK.Game.StateMachine.Input;

namespace ThreeK.Game.StateMachine.State
{
    public interface IState
    {
        Delegate OnStateEnter { get; }
        Delegate OnStateExit { get; }
        /// <summary>
        /// Handle input and returns next state.
        /// </summary>
        /// <param name="stateMachine">State machine currently belongs to</param>
        /// <param name="input">Input</param>
        /// <returns></returns>
        IState HandleInput(IInput input);
        void Enter(IInput input);
    }
}

