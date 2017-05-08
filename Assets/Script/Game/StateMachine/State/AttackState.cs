using UnityEngine;
using System.Collections;
using System;
using Adic;
using ThreeK.Game.StateMachine.Input;
using _3Kingdoms.Game.StateMachine.State;

namespace ThreeK.Game.StateMachine.State
{
    public class AttackState : MonoState
    {
        private const string ATTACK1_TRIGGER = "Attack1Trigger";

        public AttackState(IStateMachine stateMachine) : base(stateMachine as MonoBehaviour)
        {
        }

        public override void Enter(IInput input)
        {
            base.Enter(input);

            var animator = Machine.GetComponent<Animator>();
            animator.SetTrigger(ATTACK1_TRIGGER);
            StartCoroutine(Wait(1.2f));
        }

        public override IState HandleInput(IInput input)
        {
            return this;
        }

        private IEnumerator Wait(float duration)
        {
            yield return new WaitForSeconds(duration);
            OnStateExit.Invoke();
        }
    }
}
