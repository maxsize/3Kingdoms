using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adic;
using Adic.Container;
using Assets.Script.Game.Data;
using ThreeK.Game.StateMachine.Input;
using UnityEngine;
using ThreeK.Game.Helper;

namespace ThreeK.Game.StateMachine.State
{
    public class StackedState : State, IStateStack
    {
        [Inject]
        public IInjectionContainer Container;

        private readonly List<IState> _stack;
        public StackedState()
        {
            _stack = new List<IState>();
        }

        public override object Data
        {
            get { return null; }
        }

        [Inject]
        public void PostConstruct()
        {
            var input = InputHelper.CurrentInput;
            if (input != null)
            {
                _stack.AddRange(CreateStates(input));
            }
        }

        public override IState HandleInput(IInput input)
        {
            return null;
        }

        public IState[] GetStack()
        {
            return _stack.ToArray();
        }

        private List<IState> CreateStates(IInput input)
        {
            var states = new List<IState>();
            if (input is MoveInput)
            {
                // Create turn and move state
                states.Add(Container.Resolve<IState>(typeof(MoveState)));
                states.Add(Container.Resolve<IState>(typeof(TurnState)));
            }
            else if (input is AttackInput)
            {
                // Create turn and move state
                states.Add(Container.Resolve<IState>(typeof(AttackState)));
                states.Add(Container.Resolve<IState>(typeof(Move2TargetState)));
                states.Add(Container.Resolve<IState>(typeof(TurnState)));
            }
            else if (input is PreCastInput)
            {
                var ability = (AbilityVO)input.Data;
                if (ability.IsNoTarget())
                    states.Add(Container.Resolve<IState>(typeof(CastState)));
                if (ability.IsPointTarget() ||
                    ability.IsUnitTarget())
                {
                    states.Add(Container.Resolve<IState>(typeof(PreCastState)));
                }
            }
            //else if (input is PointInput || input is SelectInput)
            //{
            //    var ability = (input is PointInput) ? (input as PointInput).Ability : 
            //        (input as SelectInput).Ability;
            //    if (ability.IsPointTarget() ||
            //        (ability.IsUnitTarget() && input is SelectInput))
            //    {
            //        states.Add(Container.Resolve<IState>(typeof(CastState)));
            //        states.Add(Container.Resolve<IState>(typeof(CastingMoveState)));
            //        states.Add(Container.Resolve<IState>(typeof(TurnState)));   // Turn to target
            //    }
            //}
            //else if (input is CastInput)
            //{
            //    var cast = (CastInput) input;
            //    if (cast.Ability.AbilityTypes.Contains((int)AbilityTypes.NoTarget))
            //        states.Add(Container.Resolve<IState>(typeof(CastState)));
            //    if (cast.Ability.AbilityTypes.Contains((int)AbilityTypes.PointTarget) ||
            //        cast.Ability.AbilityTypes.Contains((int)AbilityTypes.UnitTarget))
            //    {
            //        states.Add(Container.Resolve<IState>(typeof(CastState)));
            //        states.Add(Container.Resolve<IState>(typeof(CastingMoveState)));
            //        states.Add(Container.Resolve<IState>(typeof(TurnState)));   // Turn to target
            //    }
            //}
            return states;
        }
    }
}
