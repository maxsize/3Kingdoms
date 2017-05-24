using Assets.Script.Game.Data;
using UnityEngine;

namespace ThreeK.Game.StateMachine.Input
{
    public class CastInput : GameInput
    {
        public Transform Target { get; private set; }   // For UnitTarget
        public Vector3 Point { get; private set; }      // For PointTarget

        public CastInput(Transform target, AbilityVO ability) : base(ability)
        {
            Target = target;
        }

        public CastInput(Vector3 point, AbilityVO ability) : base(ability)
        {
            Point = point;
        }
    }
}
