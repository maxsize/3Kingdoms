using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adic;
using Adic.Container;
using ThreeK.Game.StateMachine.Input;
using ThreeK.Game.StateMachine.State;
using ThreeK.Game.Helper;

namespace ThreeK.Game.StateMachine
{
    [InjectFromContainer(BindingHelper.Identifiers.MainContainer)]
    public class PushdownAutomation : StateMachine
    {
        protected List<IState> Stack;

        [Inject]
        public override void Construct()
        {
            base.Construct();
            Stack = new List<IState>();
        }

        public override IState HandleInput(IInput input)
        {
            var oldState = CurrentState;
            Stack.Add(oldState);

            var next = CurrentState.HandleInput(input);
            if (next is IStateStack)
            {
                Stack.AddRange((next as IStateStack).GetStack());
                next = Stack[Stack.Count - 1];
                Stack.Remove(next);
            }

            OnStateChange(next, input);

            return next;
        }

        protected virtual void OnStateChange(IState newState, IInput input)
        {
            CurrentState = newState;
            newState.Enter(input);
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
