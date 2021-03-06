﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adic;
using Adic.Container;
using Assets.Script.Game.Data;
using ThreeK.Game.Helper;
using ThreeK.Game.StateMachine.Input;
using UnityEngine;

namespace ThreeK.Game.StateMachine.State
{
    public class PreCastState : State, IStateStack
    {
        [Inject] public IInjectionContainer Container;

        private readonly AbilityVO _ability;
        private object _data;
        private readonly List<IState> _states = new List<IState>();

        public PreCastState()
        {
            _ability = (AbilityVO)InputHelper.CurrentInput.Data;
        }

        public override IState HandleInput(IInput input)
        {
            if (input is PointInput) _data = ((PointInput)input).Data;
            else if (input is SelectInput) _data = ((SelectInput)input).Data;

            if (string.IsNullOrEmpty(_ability.Name))
                return this;
            if (_ability.IsPointTarget() && (input is PointInput || input is SelectInput))
            {
                _states.Add(Container.Resolve<IState>(typeof(CastState)));
                _states.Add(Container.Resolve<IState>(typeof(CastingMoveState)));
                _states.Add(Container.Resolve<IState>(typeof(TurnState)));
            }
            else if (_ability.IsUnitTarget() && input is SelectInput)
            {
                _states.Add(Container.Resolve<IState>(typeof(CastState)));
                _states.Add(Container.Resolve<IState>(typeof(CastingMoveState)));
                _states.Add(Container.Resolve<IState>(typeof(TurnState)));
            }
            return this;
        }

        public IState[] GetStack()
        {
            return _states.ToArray();
        }

        public override object Data
        {
            get { return _data; }
        }
    }
}
