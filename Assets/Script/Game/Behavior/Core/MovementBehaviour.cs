using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;

namespace ThreeK.Game.Behavior.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class MovementBehaviour : MonoBehaviour
    {
        public UnityEvent OnEnd = new UnityEvent();

        protected virtual void Start()
        {
        }

        public virtual void SetTarget(object target)
        {
            SetTarget(target, 0);
        }

        public virtual void SetTarget(object target, float latency)
        {
            if (target is Quaternion) SetTarget((Quaternion)target, latency);
            if (target is Vector3) SetTarget((Vector3)target, latency);
            if (target is Transform) SetTarget((Transform)target, latency);
            if (target == null) SetTarget();
            enabled = true;
        }

        protected virtual void SetTarget()
        {
        }

        protected virtual void SetTarget(Quaternion target, float latency)
        {
            throw new NotImplementedException();
        }

        protected virtual void SetTarget(Vector3 target, float latency)
        {
            throw new NotImplementedException();
        }

        protected virtual void SetTarget(Transform target, float latency)
        {
            throw new NotImplementedException();
        }
    }
}
