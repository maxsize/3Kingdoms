using ThreeK.Game.Helper;
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
            get { return _target.position; }
        }
    }
}
