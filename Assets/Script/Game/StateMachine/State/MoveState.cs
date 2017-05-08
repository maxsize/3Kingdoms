using UnityEngine;
using System.Collections;
using System;
using Adic;
using ThreeK.Game.StateMachine.Input;
using Adic.Container;

namespace ThreeK.Game.StateMachine.State
{
    public class MoveState : State
    {
        [Inject] public IInjectionContainer Container;

        private float _moveSpeed = 10;
        private Vector3 _target;

        public MoveState(IMonoStateMachine stateMachine) : base(stateMachine)
        {
        }

        public override void Enter(IInput input)
        {
            base.Enter(input);
            _target = (Vector3) input.Data;
            var transform = this.stateMachine.Client;
            var animator = transform.GetComponent<Animator>();
            animator.SetBool("Moving", true);
            animator.SetBool("Running", true);
            StartCoroutine(MoveTowards());
        }

        public override IState HandleInput(IInput input)
        {
            if (input is AttackInput)
                return Container.Resolve<AttackState>();
            if (input is MoveInput)
                return Container.Resolve<MoveState>();
            return Container.Resolve<IdleState>();
        }

        private IEnumerator MoveTowards()
        {
            var transform = this.stateMachine.Client.transform;
            while (true)
            {
                transform.position = Vector3.MoveTowards(transform.position, _target, _moveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, _target) < 0.03)
                {
                    transform.position = _target;
                    Invoke(OnStateExit);
                    yield break;
                }
                yield return null;
            }
        }
    }
}
