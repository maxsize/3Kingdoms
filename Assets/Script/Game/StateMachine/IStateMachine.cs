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
        void HandleInput(IInput input);
    }
}
