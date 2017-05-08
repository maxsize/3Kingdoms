using Adic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using ThreeK.Game.StateMachine.Input;
using ThreeK.Game.StateMachine;

public class GameRoot : ContextRoot
{
    public override void SetupContainers()
    {
        var container = AddContainer<InjectionContainer>();
        container.RegisterExtension<UnityBindingContainerExtension>()
            .RegisterExtension<CommanderContainerExtension>()
            .RegisterExtension<EventCallerContainerExtension>()
            .Bind<IInput>().ToFactory<InputFactory>().As(typeof(MoveInput))
            .Bind<IStateMachine>().ToPrefab<Player>("Warrior3/Hammer/Prefabs/Hammer").As("Hammer")
            .Bind<IStateMachine>().ToPrefab<Player>("Warrior3/Crossbow/Prefabs/Crossbow").As("Crossbow")
            .Bind<IStateMachine>().ToPrefab<Player>("Warrior3/Swordsman/Prefabs/Swordsman").As("Swordsman")
            .Bind<ContextRoot>().To(this)
            .Bind<GameController>().ToSingleton();
    }

    public override void Init()
    {
    }
}

