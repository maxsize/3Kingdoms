using UnityEngine;
using System.Collections;
using System;
using ThreeK.Game.StateMachine.Input;
using ThreeK.Game.StateMachine.State;
using Adic;
using Adic.Container;
using ThreeK.Game.StateMachine;

namespace ThreeK.Game.StateMachine.State
{
    public class TurnState : MonoState
    {
        [Inject] public IInjectionContainer Container;

        private float _turnSpeed = 400;
        private Vector3 _target;

        public TurnState(IStateMachine machine) : base(machine as MonoBehaviour)
        {
        }

        public override IState HandleInput(IInput input)
        {
            if (input is AttackInput || input is MoveInput)
            {
                Machine.StopCoroutine("TurnTowards");
                OnStateExit.Invoke();
                return Container.Resolve<IState>(typeof(StackedState));
            }
            return this;
        }

        public override void Enter(IInput input)
        {
            base.Enter(input);
            _target = GetTarget(input);
            //_target.y = 0;
            var animator = Machine.gameObject.GetComponent<Animator>();
            animator.SetBool("Moving", false);
            animator.SetBool("Running", false);
            Machine.StopAllCoroutines();
            StartCoroutine(TurnTowards());
        }

        private Vector3 GetTarget(IInput input)
        {
            if (input.Data is Vector3)
                return (Vector3) input.Data;
            if (input.Data is Transform)
                return ((Transform) input.Data).position;
            return Machine.transform.position;
        }

        private IEnumerator TurnTowards()
        {
            var rigi = Machine.GetComponent<Rigidbody>();
            var trans = Machine.transform;
            var q = Quaternion.LookRotation(_target - rigi.position);
            q.x = q.z = 0;
            rigi.velocity = Vector3.zero;
            var animator = Machine.gameObject.GetComponent<Animator>();
            animator.SetBool("Moving", false);
            animator.SetBool("Running", false);

            while (true)
            {
                var r = Quaternion.RotateTowards(trans.rotation, q, _turnSpeed * Time.deltaTime);
                trans.rotation = r;
                var diff = Mathf.Abs(trans.eulerAngles.y - q.eulerAngles.y);
                //Debug.Log(string.Format("{0}, {1}, {2}", diff, trans.rotation.eulerAngles.y, q.eulerAngles.y));
                if (diff < 1)
                {
                    OnStateExit.Invoke();
                    break;
                }
                yield return null;
            }
        }
    }
}
