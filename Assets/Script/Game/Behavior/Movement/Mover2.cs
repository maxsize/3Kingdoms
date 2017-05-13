using UnityEngine;
using System.Collections;

namespace ThreeK.Game.Behavior.Movement
{
    public class Mover2 : Mover
    {
        private Transform _targetTrans;

        public override void SetTarget(Transform target)
        {
            _targetTrans = target;
            StartMovement();
        }

        protected override bool IsReached()
        {
            var dist = Vector3.Distance(transform.position, _targetTrans.position);
            //Debug.Log(string.Format("{0} {1} {2}", dist, trans.position, _target));
            return dist < 1;
        }
    }
}
