using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Script.Game.Data;
using ThreeK.Game.Helper;
using ThreeK.Game.StateMachine.Input;
using UnityEngine;

namespace ThreeK.Game.StateMachine.State
{
    public class CastState : State
    {
        public AbilityVO Ability { get; private set; }
        public Transform Target { get; private set; }
        public Vector3 Point { get; private set; }

        public CastState()
        {
            var input = InputHelper.CurrentInput;
            if (input is CastInput)
            {
                Point = ((CastInput)input).Point;
                Target = ((CastInput)input).Target;
                Ability = ((CastInput)input).Ability;
            }
            else if (input is PointInput)
            {
                Point = (Vector3)((PointInput)input).Data;
                Ability = ((PointInput)input).Ability;
            }
            else if (input is SelectInput)
            {
                Target = ((SelectInput)input).Data as Transform;
                Ability = ((SelectInput)input).Ability;
            }
        }

        public override IState HandleInput(IInput input)
        {
            return null;    // Casting cannot be break
        }

        public override object Data
        {
            get
            {
                if (Ability.IsNoTarget())
                    return null;
                if (Ability.IsPointTarget())
                    return Point;
                if (Ability.IsUnitTarget())
                    return Target;
                return null;
            }
        }
    }
}
