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
            .Bind<IInput>().ToFactory<InputFactory>()
            .Bind<IMonoStateMachine>().ToFactory<StateMachine.Factory>()
            .Bind<Player>().ToGameObject("Player")
            .Bind<Follower>().ToGameObject("Main Camera");
    }

    public override void Init()
    {
    }
}
