using UnityEngine;
using System.Collections;
using _3Kingdoms.Game.StateMachine.State;
using System;
using ThreeK.Game.StateMachine.Input;
using ThreeK.Game.StateMachine.State;
using Adic;
using Adic.Container;
using ThreeK.Game.StateMachine;

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
        if (input is EmptyInput) return Container.Resolve<IdleState>();
        return this;
    }

    public override void Enter(IInput input)
    {
        base.Enter(input);
        _target = (Vector3)input.Data;
        //_target.y = 0;
        var animator = Machine.gameObject.GetComponent<Animator>();
        animator.SetBool("Moving", false);
        animator.SetBool("Running", false);
        Machine.StopAllCoroutines();
        StartCoroutine(TurnTowards());
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
            var diff = Mathf.Abs(trans.rotation.y - q.y);
            //Debug.Log(string.Format("{0}, {1}, {2}", diff, trans.rotation.y, q.y));
            if (diff < 0.03)
            {
                q.x = q.z = 0;
                trans.rotation = q;
                OnStateExit.Invoke();
                break;
            }
            yield return null;
        }
    }
}
