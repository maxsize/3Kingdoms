using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ThreeK.Game.StateMachine.State
{
    public interface IStateStack : IState
    {
        IState[] GetStack();
    }
}
