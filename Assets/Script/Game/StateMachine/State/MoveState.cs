using UnityEngine;
using System.Collections;
using System;
using Adic;
using ThreeK.Game.StateMachine.Input;
using Adic.Container;
using _3Kingdoms.Game.StateMachine.State;

namespace ThreeK.Game.StateMachine.State
{
    public class MoveState : MonoState
    {
        [Inject] public IInjectionContainer Container;

        private float _moveSpeed = 10;
        private Vector3 _target;

        public MoveState(IStateMachine stateMachine) : base(stateMachine as MonoBehaviour)
        {
        }

        public override void Enter(IInput input)
        {
            base.Enter(input);
            _target = (Vector3) input.Data;
            //_target.y = 0;
            var animator = Machine.gameObject.GetComponent<Animator>();
            animator.SetBool("Moving", true);
            animator.SetBool("Running", true);
            StartCoroutine(MoveTowards());
        }

        public override IState HandleInput(IInput input)
        {
            if (input is AttackInput || input is MoveInput)
            {
                Machine.StopCoroutine("MoveTowards");
                OnStateExit.Invoke();
                return Container.Resolve<IState>(typeof(StackedState));
            }
            return Container.Resolve<IState>(typeof(IdleState));
        }

        private IEnumerator MoveTowards()
        {
            var rigi = Machine.GetComponent<Rigidbody>();
            var trans = Machine.transform;
            var animator = Machine.gameObject.GetComponent<Animator>();

            animator.SetBool("Moving", true);
            animator.SetBool("Running", true);
            rigi.velocity = Vector3.forward * 10;
            while (true)
            {
                var dist = Vector3.Distance(trans.position, _target);
                Debug.Log(string.Format("{0} {1} {2}", dist, trans.position, _target));
                if (dist < 1)
                {
                    animator.SetBool("Moving", false);
                    animator.SetBool("Running", false);
                    rigi.velocity = Vector3.zero;
                    OnStateExit.Invoke();
                    yield break;
                }
                yield return null;
            }
        }
    }
}
