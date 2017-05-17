using System.Collections;
using System.Collections.Generic;
using Adic;
using Game.Behavior.Movement;
using Game.Event;
using ThreeK.Game.Behavior.Core;
using ThreeK.Game.Networking;
using UnityEngine;

namespace ThreeK.Game.Behavior.Movement
{
    public class Mover : MovementBehaviour
    {
        public float Speed = 7f;
        private Vector3 _target;
        
        protected override void SetTarget(Vector3 target, float latency)
        {
            AddListener();
            _target = target;
            transform.Translate(Vector3.forward * 10 * latency);
            StartMovement();
        }

        protected virtual bool IsReached()
        {
            var trans = transform;
            var dist = Vector3.Distance(trans.position, _target);
            //Debug.Log(string.Format("{0} {1} {2}", dist, trans.position, _target));
            return dist < 1;
        }

        protected void StartMovement()
        {
            var animator = GetComponent<Animator>();
            animator.SetBool("Moving", true);
            animator.SetBool("Running", true);
        }

        private void OnDisable()
        {
            RemoveListener();
        }

        private void FixedUpdate()
        {
            transform.Translate(Vector3.forward * Speed * Time.fixedDeltaTime);
            if (IsReached())
            {
                EndMove();
                transform.position = _target - transform.forward;   // Correct the final position
                enabled = false;
            }
        }

        private void AddListener()
        {
            if (Dispatcher != null)
                Dispatcher.AddListener<CollideEvent>(OnCollide);
        }

        private void RemoveListener()
        {
            if (Dispatcher != null)
                Dispatcher.RemoveListener<CollideEvent>(OnCollide);
        }

        private void OnCollide(CollideEvent e)
        {
            EndMove();
            //var ray = new Ray(transform.position, Vector3.up);
            Debug.DrawRay(transform.position, Vector3.up * 5, Color.white, 3);
            enabled = false;
        }

        private void EndMove()
        {
            OnEnd.Invoke();
        }
    }
}
