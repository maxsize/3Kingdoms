using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adic;
using Adic.Container;
using Assets.Script.Game.Data;
using ThreeK.Game.Helper;
using ThreeK.Game.StateMachine.Input;

namespace ThreeK.Game.StateMachine.State
{
    public class PreCastState : State
    {
        [Inject] public IInjectionContainer Container;

        private readonly AbilityVO _data;
        private readonly List<IState> _states = new List<IState>();

        public PreCastState()
        {
            _data = (AbilityVO)InputHelper.CurrentInput.Data;
        }

        public override IState HandleInput(IInput input)
        {
            if (string.IsNullOrEmpty(_data.Name))
                return this;
            if (_data.AbilityTypes.Contains((int) AbilityTypes.PointTarget) &&
                (input is PointInput || input is SelectInput))
            {
                _states.Add(Container.Resolve<IState>(typeof(Move2TargetState)));
                _states.Add(Container.Resolve<IState>(typeof(TurnState)));
            }
            return this;
        }

        public override object Data
        {
            get { return _data; }
        }
    }
}
