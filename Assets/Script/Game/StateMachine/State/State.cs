
using System;
using System.Collections;
using ThreeK.Game.StateMachine.Input;

namespace ThreeK.Game.StateMachine.State
{
    public abstract class State : IState
    {
        delegate void StateEnterDel();
        delegate void StateExitDel();

        StateEnterDel _onStateEnter;
        StateExitDel _onStateExit;

        protected IMonoStateMachine stateMachine;

        public State(IMonoStateMachine stateMachine)
        {
            this.stateMachine = stateMachine;
        }

        public virtual void Enter(IInput input)
        {
            Invoke(_onStateEnter);
        }

        public abstract IState HandleInput(IInput input);

        public Delegate OnStateEnter
        {
            get { return _onStateEnter; }
        }

        public Delegate OnStateExit
        {
            get { return _onStateExit; }
        }

        protected void Invoke(Delegate del)
        {
            if (del != null)
                del.DynamicInvoke();
        }

        protected void StartCoroutine(IEnumerator routine)
        {
            stateMachine.Client.StartCoroutine(routine);
        }
    }
}
