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

    public float FULL_TURN_TIME = 0.75f;

    private float _turnSpeed = 100;
    private Vector3 _target;
    private float _maxTurningTime;
    private float _turningTime;

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
        var angle = Mathf.Abs(q.eulerAngles.y - rigi.rotation.eulerAngles.y);
        angle = angle > 180 ? 360 - angle : angle;
        _maxTurningTime = (angle / 180) * FULL_TURN_TIME;
        _turningTime = 0;
        rigi.velocity = Vector3.zero;

        while (true)
        {
            var r = Quaternion.RotateTowards(trans.rotation, q, _turnSpeed * Time.deltaTime);
            r.x = r.z = 0;
            trans.rotation = r;
            _turningTime += Time.deltaTime;
            //if (_turningTime >= _maxTurningTime)
            var diff = Mathf.Abs(trans.rotation.y - q.y);
            Debug.Log(string.Format("{0}, {1}, {2}", diff, trans.rotation.y, q.y));
            if (diff < 0.03)
            {
                break;
            }
            yield return null;
        }

        var animator = Machine.gameObject.GetComponent<Animator>();
        animator.SetBool("Moving", true);
        animator.SetBool("Running", true);
        rigi.velocity = Vector3.forward * 10;
        while (true)
        {
            var dist = Vector3.Distance(trans.position, _target);
            Debug.Log(string.Format("{0} {1} {2}", dist, trans.position, _target));
            if (dist < 0.03)
            {
                OnStateExit.Invoke();
                rigi.velocity = Vector3.zero;
                yield break;
            }
            yield return null;
        }
    }
}
