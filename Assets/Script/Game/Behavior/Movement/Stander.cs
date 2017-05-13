using UnityEngine;
using System.Collections;
using ThreeK.Game.Behavior.Core;

public class Stander : MovementBehaviour
{

    public override void SetTarget()
    {
        var animator = GetComponent<Animator>();
        animator.SetBool("Moving", false);
        animator.SetBool("Running", false);
        var rigi = GetComponent<Rigidbody>();
        rigi.velocity = Vector3.zero;
    }
}
