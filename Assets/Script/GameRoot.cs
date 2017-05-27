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
using ThreeK.Game.Data;
using ThreeK.Game.Event;
using System.IO;
using Assets.Script.Game.Data;

public class GameRoot : ContextRoot
{
    private IInjectionContainer _container;
    public override void SetupContainers()
    {
        var jsonStr = File.ReadAllText("Assets/Resources/Data/Abilities");
        var meta = new Metadata()
        {
            Abilities = JsonUtility.FromJson<AbilityVO[]>(jsonStr)
        };

        _container = AddContainer<InjectionContainer>(BindingHelper.Identifiers.MainContainer);
        _container.RegisterExtension<UnityBindingContainerExtension>()
            .RegisterExtension<CommanderContainerExtension>()
            .RegisterExtension<EventCallerContainerExtension>()
            .Bind<Metadata>().To(meta)
            .Bind<ContextRoot>().To(this)
            .Bind<EventDispatcher>().ToSingleton()
            .Bind<InputFactory>().ToSingleton()
            .Bind<PlayerVO>().ToSingleton()
            .Bind<AttackInput>().ToFactory<InputFactory>()
            .Bind<MoveInput>().ToFactory<InputFactory>()
            //.Bind<IStateMachine>().ToPrefab<Player>("Warrior3/Hammer/Prefabs/Hammer").As("Hammer")
            //.Bind<IStateMachine>().ToPrefab<Player>("Warrior3/Crossbow/Prefabs/Crossbow").As("Crossbow")
            //.Bind<IStateMachine>().ToPrefab<Player>("Warrior3/Swordsman/Prefabs/Swordsman").As("Swordsman")
            .Bind<NetworkManager>().ToGameObject("NetworkManager")
            .Bind<GameController>().ToSingleton();
    }

    public override void Init()
    {
        _container.Resolve<GameController>();
    }
}

