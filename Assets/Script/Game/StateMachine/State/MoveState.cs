using UnityEngine;
using System.Collections;
using System;
using Adic;
using ThreeK.Game.StateMachine.Input;
using Adic.Container;
using _3Kingdoms.Game.StateMachine.State;

namespace ThreeK.Game.StateMachine.State
{
    [InjectFromContainer("MainContainer")]
    public class MoveState : MonoState
    {
        [Inject("SubContainer")] public IInjectionContainer Container;

        private float _moveSpeed = 10;
        private Vector3 _target;

        public MoveState(IStateMachine stateMachine) : base(stateMachine as MonoBehaviour)
        {
        }

        public override void Enter(IInput input)
        {
            base.Enter(input);
            _target = (Vector3) input.Data;
            var animator = Machine.gameObject.GetComponent<Animator>();
            animator.SetBool("Moving", true);
            animator.SetBool("Running", true);
            StartCoroutine(MoveTowards());
        }

        public override IState HandleInput(IInput input)
        {
            if (input is AttackInput)
                return Container.Resolve<IState>(typeof(AttackState));
            if (input is MoveInput)
                return Container.Resolve<IState>(typeof(MoveState));
            return Container.Resolve<IState>(typeof(IdleState));
        }

        private IEnumerator MoveTowards()
        {
            var transform = Machine.transform;
            while (true)
            {
                transform.position = Vector3.MoveTowards(transform.position, _target, _moveSpeed * Time.deltaTime);
                if (Vector3.Distance(transform.position, _target) < 0.03)
                {
                    transform.position = _target;
                    OnStateExit.Invoke();
                    yield break;
                }
                yield return null;
            }
        }
    }
}
