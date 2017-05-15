using System.Collections.Generic;
using Adic;
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
        public void Construct()
        {
            Stack = new List<IState>();
        }

        public override IState HandleInput(IInput input)
        {
            var oldState = CurrentState;
            if (Stack.Count == 0)
                Stack.Add(oldState);    // Only keep the last state (idle state)

            var next = CurrentState.HandleInput(input);
            if (next is IStateStack)
            {
                Stack.AddRange((next as IStateStack).GetStack());
                next = Stack[Stack.Count - 1];
                Stack.Remove(next);
            }

            ChangeState(next);
            return next;
        }

        public bool NextState()
        {
            if (Stack.Count == 0)
                return false;
            var next = Stack[Stack.Count - 1];
            Stack.Remove(next);         // Pop last state (current state)
            ChangeState(next);    // Update current state
            return true;
        }
    }
}
