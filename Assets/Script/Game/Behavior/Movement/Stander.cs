using UnityEngine;
using System.Collections;
using ThreeK.Game.Behavior.Core;

public class Stander : MovementBehaviour
{

    protected override void SetTarget()
    {
        var animator = GetComponent<Animator>();
        animator.SetBool("Moving", false);
        animator.SetBool("Running", false);
    }
}
