using UnityEngine;
using System.Collections;
using ThreeK.Game.StateMachine.Input;
using System;

namespace ThreeK.Game.UI
{
    public class HeroSelectionInput : GameInput
    {
        public HeroSelectionInput(object data = null) : base(data)
        {
        }
    }

    public class LobbyInput : GameInput
    {
        public LobbyInput(object data = null) : base(data)
        {
        }
    }

    public class GameSceneInput : GameInput
    {
        public GameSceneInput(object data = null) : base(data)
        {
        }
    }
}
