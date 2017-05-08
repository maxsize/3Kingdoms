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

        private List<IState> _stack;

        [Inject]
        public override void Construct()
        {
            base.Construct();
            _stack = new List<IState>();
        }

        public override void AddStates(IState[] states, IState defaultState)
        {
            base.AddStates(states, defaultState);
            if (_stack.Count == 0)
                _stack.Add(defaultState);
        }

        public override IState HandleInput(IInput input)
        {
            var oldState = CurrentState;
            var next = base.HandleInput(input);
            _stack.Add(oldState);
            _stack.Add(next);
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

        private void Pop()
        {
            _stack.RemoveAt(_stack.Count - 1);          // Pop last state (current state)
            CurrentState = _stack[_stack.Count - 1];    // Update current state
            CurrentState.Enter(Container.Resolve<EmptyInput>());
        }
    }
}
