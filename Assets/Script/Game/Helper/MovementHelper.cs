using UnityEngine;
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
                data.MovementType = (state is Move2TargetState) ? typeof(Mover2) : typeof(Mover);
            else if (state is AttackState)
                data.MovementType = typeof(Attacker);
            else if (state is CastingMoveState)
            {
                data.MovementType = typeof(CastingMover);
                data.Ability = ((CastingMoveState) state).Ability;
            }
            else if (state is CastState)
            {
                data.MovementType = typeof(CastingMover);
                data.Ability = ((CastState)state).Ability;
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
