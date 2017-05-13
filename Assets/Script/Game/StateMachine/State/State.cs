
using System;
using System.Collections;
using ThreeK.Game.StateMachine.Input;
using UnityEngine.Events;

namespace ThreeK.Game.StateMachine.State
{
    public abstract class State : IState
    {
        public abstract object Data { get; }

        protected State()
        {
        }

        public abstract IState HandleInput(IInput input);
    }
}
