using UnityEngine;
using System.Collections;
using ThreeK.Game.Behavior.Core;

public class Stander : MovementBehaviour
{
    private Vector3 _position = Vector3.zero;

    private void FixedUpdate()
    {
        if (_position != Vector3.zero)
            transform.position = _position;
    }

    private void OnDisable()
    {
        _position = Vector3.zero;
    }

    public override void End()
    {
        enabled = false;
    }

    protected override void SetTarget()
    {
        var animator = GetComponent<Animator>();
        animator.SetBool("Moving", false);
        animator.SetBool("Running", false);
        _position = transform.position;
    }
}
