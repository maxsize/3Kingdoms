using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeK.Game.StateMachine.Input;
using UnityEngine;

namespace ThreeK.Game.StateMachine.State
{
    public class Move2TargetState : MoveState
    {
        private Transform _target;

        public Move2TargetState(IStateMachine stateMachine) : base(stateMachine)
        {
        }

        protected override Vector3 GetDestination(IInput input)
        {
            if (!(input.Data is Transform))
                return Machine.transform.position;

            _target = (Transform) input.Data;
            return _target.position;
        }

        protected override bool IsReached()
        {
            var trans = Machine.transform;
            var dist = Vector3.Distance(trans.position, _target.position);
            Debug.Log(string.Format("{0} {1} {2}", dist, trans.position, _target.position));
            return dist < 2;
        }
    }
}
