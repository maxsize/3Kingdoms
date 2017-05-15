using UnityEngine;
using System.Collections;
using ThreeK.Game.Behavior.Core;

public class Attacker : MovementBehaviour
{
    private const string ATTACK1_TRIGGER = "Attack1Trigger";

    public override void SetTarget(Transform target)
    {

        var animator = GetComponent<Animator>();
        animator.SetTrigger(ATTACK1_TRIGGER);
        StartCoroutine(Wait(1.2f));
    }

    private IEnumerator Wait(float duration)
    {
        yield return new WaitForSeconds(duration);
        OnEnd.Invoke();
        enabled = false;
    }
}
