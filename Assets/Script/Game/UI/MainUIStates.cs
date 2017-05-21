using UnityEngine;
using System.Collections;
using ThreeK.Game.UI;
using ThreeK.Game.StateMachine.Input;
using ThreeK.Game.StateMachine.State;
using System;

namespace ThreeK.Game.UI
{
    public class HeroSelectionState : BaseUIState
    {
        public override IState HandleInput(IInput input)
        {
            if (input is LobbyInput)
                return new LobbyState();
            return this;
        }

        public override object Data
        {
            get
            {
                return "HeroSelection";
            }
        }
    }

    public class LobbyState : BaseUIState
    {
        public override IState HandleInput(IInput input)
        {
            if (input is HeroSelectionInput)
                return new HeroSelectionState();
            return this;
        }

        public override object Data
        {
            get
            {
                return "Lobby";
            }
        }
    }

    public class BaseUIState : State
    {

        public override object Data
        {
            get { throw new NotImplementedException(); }
        }

        public override IState HandleInput(IInput input)
        {
            throw new NotImplementedException();
        }
    }
}
