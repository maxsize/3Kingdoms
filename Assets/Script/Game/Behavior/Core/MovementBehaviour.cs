using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

namespace ThreeK.Game.Behavior.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovementBehaviour : MonoBehaviour
    {
        public UnityEvent OnEnd;

        protected virtual void Start()
        {
            OnEnd = new UnityEvent();
        }

        public virtual void SetTarget(object target)
        {
            if (target is Quaternion) SetTarget((Quaternion)target);
            if (target is Vector3) SetTarget((Vector3)target);
            if (target is Transform) SetTarget((Transform)target);
            if (target == null) SetTarget();
        }

        public virtual void SetTarget()
        {
        }

        public virtual void SetTarget(Quaternion target)
        {
            throw new NotImplementedException();
        }

        public virtual void SetTarget(Vector3 target)
        {
            throw new NotImplementedException();
        }

        public virtual void SetTarget(Transform target)
        {
            throw new NotImplementedException();
        }
    }
}
