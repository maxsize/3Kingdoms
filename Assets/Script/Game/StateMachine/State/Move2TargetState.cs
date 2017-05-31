using ThreeK.Game.Helper;
using ThreeK.Game.StateMachine.Input;
using UnityEngine;

namespace ThreeK.Game.StateMachine.State
{
    public class Move2TargetState : MoveState
    {
        private Transform _target;

        public Move2TargetState()
        {
            _target = (Transform)InputHelper.CurrentInput.Data;
        }

        public override object Data
        {
            get { return _target; }
        }

        protected override Vector3 GetDestination(IInput input)
        {
            if (input.Data is Vector3) return (Vector3)input.Data;
            return ((Transform)input.Data).position;
        }
    }
}
