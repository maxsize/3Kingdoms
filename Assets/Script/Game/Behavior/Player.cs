using System.Collections.Generic;
using Adic;
using Adic.Container;
using ThreeK.Game.StateMachine;
using ThreeK.Game.StateMachine.Input;
using ThreeK.Game.StateMachine.State;
using UnityEngine;

[InjectFromContainer("MainContainer")]
public class Player : PushdownAutomation
{
    [Inject] public IInjectionContainer MainContainer;
    [Inject] public ContextRoot Context;

    private IInjectionContainer _subContainer;

    protected override void Start()
    {
        base.Start();
        PostConstruct();
    }

    void PostConstruct()
    {
        _subContainer = Context.AddContainer<InjectionContainer>();
        _subContainer.RegisterExtension<UnityBindingContainerExtension>();
        MainContainer.Bind<IInjectionContainer>().To(_subContainer).As("SubContainer");

        _subContainer.Bind<IStateMachine>().To(this)
            .Bind<IState>().To<IdleState>().As(typeof(IdleState))
            .Bind<IState>().To<MoveState>().As(typeof(MoveState))
            .Bind<IState>().To<TurnState>().As(typeof(TurnState))
            .Bind<IState>().To<AttackState>().As(typeof(AttackState));


        List<IState> states = new List<IState>
        {
            _subContainer.Resolve<IState>(typeof(IdleState)),
            _subContainer.Resolve<IState>(typeof(MoveState)),
            _subContainer.Resolve<IState>(typeof(TurnState)),
            _subContainer.Resolve<IState>(typeof(AttackState))
        };
        AddStates(states.ToArray(), states[0]);
    }

    // Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
		    var input = MainContainer.Resolve<IInput>(typeof(MoveInput));
            if (input != null)
                HandleInput(input);
        }
	}

    protected override void Pop()
    {
        Stack.RemoveAt(Stack.Count - 1);          // Pop last state (current state)
        CurrentState = Stack[Stack.Count - 1];    // Update current state
        CurrentState.Enter(_subContainer.Resolve<EmptyInput>());
    }
}
