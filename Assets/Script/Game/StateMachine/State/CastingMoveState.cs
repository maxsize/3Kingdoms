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
    public class CastingMoveState : MoveState
    {
        public AbilityVO Ability { get; private set; }

        private readonly Vector3 _point;
        private readonly Transform _target;

        public CastingMoveState()
        {
            var input = InputHelper.CurrentInput;
            if (input is PointInput)
            {
                _point = (Vector3)input.Data;
                Ability = ((PointInput)input).Ability;
            }
            else if (input is SelectInput)
            {
                _target = (Transform)input.Data;
                Ability = ((SelectInput)input).Ability;
            }
        }

        public override object Data
        {
            get
            {
                if (Ability.IsPointTarget())
                    return _point;
                if (Ability.IsUnitTarget())
                    return _target;
                return null;
            }
        }

        protected override Vector3 GetDestination(IInput input)
        {
            return _target != null ? _target.position : _point;
        }
    }
}
