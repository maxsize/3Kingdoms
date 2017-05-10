using Adic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ThreeK.Game.StateMachine.Input;
using ThreeK.Game.StateMachine;
using ThreeK.Game.Behavior;
using Adic.Container;

public class GameRoot : ContextRoot
{
    private IInjectionContainer _container;
    public override void SetupContainers()
    {
        _container = AddContainer<InjectionContainer>("MainContainer");
        _container.RegisterExtension<UnityBindingContainerExtension>()
            .RegisterExtension<CommanderContainerExtension>()
            .RegisterExtension<EventCallerContainerExtension>()
            .Bind<NetworkManager>().ToGameObject("NetworkManager")
            .Bind<IInput>().ToFactory<InputFactory>().As(typeof(MoveInput))
            .Bind<IStateMachine>().ToPrefab<Player>("Warrior3/Hammer/Prefabs/Hammer").As("Hammer")
            .Bind<IStateMachine>().ToPrefab<Player>("Warrior3/Crossbow/Prefabs/Crossbow").As("Crossbow")
            .Bind<IStateMachine>().ToPrefab<Player>("Warrior3/Swordsman/Prefabs/Swordsman").As("Swordsman")
            .Bind<ContextRoot>().To(this)
            .Bind<GameController>().ToSingleton();
    }

    public override void Init()
    {
        _container.Resolve<GameController>();
    }
}

