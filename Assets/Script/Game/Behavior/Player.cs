using System;
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
    [Inject]
    public IInjectionContainer MainContainer;
    [Inject]
    public ContextRoot Context;

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
            .Bind<IState>().To<StackedState>().As(typeof(StackedState))
            .Bind<IState>().To<AttackState>().As(typeof(AttackState));


        List<IState> states = new List<IState>
        {
            _subContainer.Resolve<IState>(typeof(IdleState))
        };
        AddStates(states.ToArray(), states[0]);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (CreateAndHandleInput(typeof(AttackInput))) return;
            if (CreateAndHandleInput(typeof(MoveInput))) return;
        }
    }

    private bool CreateAndHandleInput(Type inputType)
    {
        var input = MainContainer.Resolve<IInput>(inputType);
        if (input != null)
            HandleInput(input);
        return input != null;
    }

    protected override void Pop()
    {
        Stack.RemoveAt(Stack.Count - 1);          // Pop last state (current state)
        CurrentState = Stack[Stack.Count - 1];    // Update current state
        CurrentState.Enter(_subContainer.Resolve<EmptyInput>());
    }
}
