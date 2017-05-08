
using System;
using System.Collections;
using ThreeK.Game.StateMachine.Input;
using UnityEngine.Events;

namespace ThreeK.Game.StateMachine.State
{
    public abstract class State : IState
    {
        public UnityEvent OnStateEnter { get; private set; }
        public UnityEvent OnStateExit { get; private set; }

        protected State()
        {
            OnStateEnter = new UnityEvent();
            OnStateExit = new UnityEvent();
        }

        public virtual void Enter(IInput input)
        {
            OnStateEnter.Invoke();
        }

        public abstract IState HandleInput(IInput input);
    }
}
