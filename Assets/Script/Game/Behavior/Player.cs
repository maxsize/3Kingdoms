using System.Collections.Generic;
using Adic;
using Adic.Container;
using ThreeK.Game.StateMachine;
using ThreeK.Game.StateMachine.Input;
using ThreeK.Game.StateMachine.State;
using UnityEngine;

public class Player : PushdownAutomation
{
    [Inject] public IInjectionContainer Container;

    protected override void Start()
    {
        base.Start();
        PostConstruct();
    }

    void PostConstruct()
    {
        var context = FindObjectOfType<ContextRoot>();
        var subContainer = context.AddContainer<InjectionContainer>();
        subContainer.RegisterExtension<UnityBindingContainerExtension>();
        Container.Bind<IInjectionContainer>().To(subContainer).As("SubContainer");

        subContainer.Bind<IStateMachine>().To(this)
            .Bind<IState>().To<IdleState>().As(typeof(IdleState))
            .Bind<IState>().To<MoveState>().As(typeof(MoveState))
            .Bind<IState>().To<AttackState>().As(typeof(AttackState));


        List<IState> states = new List<IState>
        {
            subContainer.Resolve<IState>(typeof(IdleState)),
            subContainer.Resolve<IState>(typeof(MoveState)),
            subContainer.Resolve<IState>(typeof(AttackState))
        };
        AddStates(states.ToArray(), states[0]);
    }

    // Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
		    var input = Container.Resolve<IInput>(typeof(MoveInput));
            if (input != null)
                HandleInput(input);
        }
	}
}
