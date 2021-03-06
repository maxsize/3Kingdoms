﻿using UnityEngine;
using System.Collections;
using System;
using Assets.Script.Game.Data;
using ThreeK.Game.StateMachine.State;
using ThreeK.Game.Behavior.Movement;

namespace ThreeK.Game.Helper
{
    public class MovementHelper
    {
        public static MovementData GetMovementData(IState state)
        {
            var data = new MovementData()
            {
                Data = state.Data
            };
            if (state is TurnState)
                data.MovementType = typeof(Spinner);
            else if (state is MoveState)
            {
                if (state is CastingMoveState)
                {
                    var ability = ((CastingMoveState)state).Ability;
                    data.MovementType = ability.IsUnitTarget() ?
                        typeof(CastingMover2) : typeof(CastingMover);
                    data.Ability = ability;
                }
                else
                    data.MovementType = (state is Move2TargetState) ? typeof(Mover2) : typeof(Mover);
            }
            else if (state is AttackState)
                data.MovementType = typeof(Attacker);
            else if (state is CastState)
            {
                data.Ability = ((CastState)state).Ability;
                data.MovementType = Type.GetType("ThreeK.Game.Behavior.Movement.Cast." + data.Ability.Name);
            }
            else
                data.MovementType = typeof(Stander);
            return data;
        }
    }

    public struct MovementData
    {
        public Type MovementType;
        public object Data;
        public AbilityVO Ability;   // For cast movements
    }
}
