using Adic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ThreeK.Game.StateMachine.Input;
using ThreeK.Game.StateMachine;
using ThreeK.Game.Behavior;
using Adic.Container;
using UnityEngine.Networking;
using ThreeK.Game.Helper;

public class GameRoot : ContextRoot
{
    private IInjectionContainer _container;
    public override void SetupContainers()
    {
        _container = AddContainer<InjectionContainer>(BindingHelper.Identifiers.MainContainer);
        _container.RegisterExtension<UnityBindingContainerExtension>()
            .RegisterExtension<CommanderContainerExtension>()
            .RegisterExtension<EventCallerContainerExtension>()
            .Bind<NetworkManager>().ToGameObject("NetworkManager")
            .Bind<InputFactory>().ToSingleton()
            .Bind<AttackInput>().ToFactory<InputFactory>()
            .Bind<MoveInput>().ToFactory<InputFactory>()
            //.Bind<IStateMachine>().ToPrefab<Player>("Warrior3/Hammer/Prefabs/Hammer").As("Hammer")
            //.Bind<IStateMachine>().ToPrefab<Player>("Warrior3/Crossbow/Prefabs/Crossbow").As("Crossbow")
            //.Bind<IStateMachine>().ToPrefab<Player>("Warrior3/Swordsman/Prefabs/Swordsman").As("Swordsman")
            .Bind<ContextRoot>().To(this)
            .Bind<GameController>().ToSingleton();
    }

    public override void Init()
    {
        _container.Resolve<GameController>();
    }
}

