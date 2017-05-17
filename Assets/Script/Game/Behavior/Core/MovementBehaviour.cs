using UnityEngine;
using System.Collections;
using UnityEngine.Events;
using System;
using Adic;
using Game.Behavior.Movement;
using Game.Event;
using ThreeK.Game.Helper;

namespace ThreeK.Game.Behavior.Core
{
    [InjectFromContainer(BindingHelper.Identifiers.PlayerContainer)]
    public class MovementBehaviour : MonoBehaviour
    {
        [Inject] public EventDispatcher Dispatcher;
        public UnityEvent OnEnd = new UnityEvent();

        private bool _injected;
        
        public virtual void SetTarget(object target)
        {
            SetTarget(target, 0);
        }

        public virtual void SetTarget(object target, float latency)
        {
            if (!_injected)
            {
                this.Inject();
                _injected = true;
            }
            
            if (target is Quaternion) SetTarget((Quaternion)target, latency);
            if (target is Vector3) SetTarget((Vector3)target, latency);
            if (target is Transform) SetTarget((Transform)target, latency);
            if (target == null) SetTarget();
            enabled = true;
            Dispatcher.DispatchWith<TargetChangeEvent>(GetType().Name);
        }

        /// <summary>
        /// Will be called before switch to next movement
        /// </summary>
        public virtual void End()
        {
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

    public class TargetChangeEvent : FEvent
    {
        public TargetChangeEvent(object data) : base(data)
        {
        }
    }
}
