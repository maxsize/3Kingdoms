using System.Collections.Generic;
using Adic;
using ThreeK.Game.Helper;
using UnityEngine;
using ThreeK.Game.Event;

namespace Game.Behavior.Movement
{
    [InjectFromContainer(BindingHelper.Identifiers.PlayerContainer)]
    public class CollisionDetector : MonoBehaviour
    {
        [Inject] public EventDispatcher Dispatcher;
        public float DirectionFactor = 1.5f;

        private RaycastHit _hitInfo;
        private Ray _ray;

        private void Start()
        {
            this.Inject();
        }

        private void FixedUpdate()
        {
            if (IsCollidingVertically())
                Dispatcher.DispatchWith<CollideEvent>(transform.position);
        }

        bool IsCollidingVertically()
        {
            var origin = transform.position;
            var forward = transform.forward;
            var left1 = Quaternion.Euler(0, -30, 0) * forward;
            var left2 = Quaternion.Euler(0, -60, 0) * forward;
            var right1 = Quaternion.Euler(0, 30, 0) * forward;
            var right2 = Quaternion.Euler(0, 60, 0) * forward;
            List<Vector3> list = new List<Vector3>() {left2, left1, forward, right1, right2};

            bool hitted = false;
            for (int i = 0; i < list.Count; i++)
            {
                _ray = new Ray(origin, list[i] * DirectionFactor);
                Debug.DrawRay(origin, list[i] * DirectionFactor, Color.yellow);
                hitted = hitted || Physics.Raycast(_ray, out _hitInfo, DirectionFactor);
                if (hitted) break;
            }
            return hitted;
        }
    }

    public class CollideEvent : FEvent
    {
        public CollideEvent(object position) : base(position)
        {
        }
    }
}
