﻿using Game.Behavior.Movement;
using ThreeK.Game.Behavior.Core;
using UnityEngine;

namespace ThreeK.Game.Behavior.Movement
{
    public class Mover : MovementBehaviour
    {
        public float Threashold = 0.1f;
        public float WalkSpeed = 2f;
        public float RunSpeed = 5f;
        public float TurnSpeed = 400f;

        protected float Speed = 5f;

        private Vector3 _target;
        private Animator _animator;

        protected override void SetTarget(Vector3 target, float latency)
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
            AddListener();
            _target = target;
            // Make sure facing the right direction
            transform.rotation = Quaternion.LookRotation(_target - transform.position);
            transform.Translate(Vector3.forward * Speed * latency);
            StartMovement();
        }

        public override void End()
        {
            enabled = false;
        }

        protected virtual bool IsReached()
        {
            var trans = transform;
            var dist = Vector3.Distance(trans.position, _target);
            //Debug.Log(string.Format("{0} {1} {2}", dist, trans.position, _target));
            return dist < Threashold;
        }

        protected virtual void StartMovement()
        {
            Speed = RunSpeed;
            _animator.SetBool("Moving", true);
            _animator.SetBool("Running", true);
        }

        private void OnDisable()
        {
            RemoveListener();
        }

        protected virtual void FixedUpdate()
        {
            transform.Translate(Vector3.forward * Speed * Time.fixedDeltaTime);
            if (IsReached())
            {
                //Debug.Log(transform.position + " - " + _target + " - " + Vector3.Distance(transform.position, _target));
                transform.position = _target;   // Correct the final position
                EndMove();
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
