using System;
using System.Collections.Generic;
using Adic;
using Adic.Container;
using Game.Event;
using ThreeK.Game.StateMachine;
using ThreeK.Game.StateMachine.Input;
using ThreeK.Game.StateMachine.State;
using UnityEngine;
using ThreeK.Game.Helper;
using UnityEngine.Networking;

[InjectFromContainer(BindingHelper.Identifiers.MainContainer)]
public class Player : PushdownAutomation
{
    [Inject]
    public IInjectionContainer MainContainer;
    [Inject]
    public ContextRoot Context;

    private IInjectionContainer _subContainer;

    private void Start()
    {
        this.Inject();
        PostConstruct();
    }
    
    void PostConstruct()
    {
        var networkId = GetComponent<NetworkIdentity>();

        _subContainer = Context.AddContainer<InjectionContainer>(BindingHelper.Identifiers.PlayerContainer);
        _subContainer.RegisterExtension<UnityBindingContainerExtension>();
        MainContainer.Bind<IInjectionContainer>().To(_subContainer).As(networkId.assetId);

        _subContainer.Bind<IStateMachine>().To(this)
            .Bind<EventDispatcher>().ToSingleton()
            .Bind<IState>().To<IdleState>().As(typeof(IdleState))
            .Bind<IState>().To<MoveState>().As(typeof(MoveState))
            .Bind<IState>().To<TurnState>().As(typeof(TurnState))
            .Bind<IState>().To<StackedState>().As(typeof(StackedState))
            .Bind<IState>().To<Move2TargetState>().As(typeof(Move2TargetState))
            .Bind<IState>().To<AttackState>().As(typeof(AttackState));


        AddState(_subContainer.Resolve<IState>(typeof(IdleState)));
    }
}
