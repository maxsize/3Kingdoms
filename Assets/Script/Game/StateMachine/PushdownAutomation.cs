using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adic;
using Adic.Container;
using ThreeK.Game.StateMachine.Input;
using ThreeK.Game.StateMachine.State;

namespace ThreeK.Game.StateMachine
{
    [InjectFromContainer("MainContainer")]
    public class PushdownAutomation : StateMachine
    {
        [Inject("SubContainer")] public IInjectionContainer Container;

        protected List<IState> Stack;

        [Inject]
        public override void Construct()
        {
            base.Construct();
            Stack = new List<IState>();
        }

        public override void AddStates(IState[] states, IState defaultState)
        {
            base.AddStates(states, defaultState);
            if (Stack.Count == 0)
                Stack.Add(defaultState);
        }

        public override IState HandleInput(IInput input)
        {
            var oldState = CurrentState;
            var next = base.HandleInput(input);
            Stack.Add(oldState);
            Stack.Add(next);
            return next;
        }

        protected override void OnStateChange(IState newState)
        {
            newState.OnStateExit.AddListener(OnStateExit);
        }

        private void OnStateExit()
        {
            CurrentState.OnStateExit.RemoveListener(OnStateExit);
            Pop();
        }

        protected virtual void Pop()
        {
        }
    }
}
