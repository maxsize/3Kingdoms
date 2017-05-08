using Adic;
using Adic.Container;
using System.Collections;
using System.Collections.Generic;
using ThreeK.Game.StateMachine;
using ThreeK.Game.StateMachine.Input;
using UnityEngine;

public class Player : InjectableBehaviour
{
    [Inject] public IMonoStateMachine Machine;
    [Inject] public IInjectionContainer Container;

    // Use this for initialization
    protected override void Start()
    {
        base.Start();
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var input = Container.Resolve<MoveInput>();
            if (input != null)
                Machine.HandleInput(input);
        }
	}
}
