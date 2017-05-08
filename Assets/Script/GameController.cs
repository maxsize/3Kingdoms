using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adic;
using ThreeK.Game.StateMachine;

namespace Assets.Script
{
    public class GameController
    {
        private IStateMachine _player;

        public GameController([Inject("Crossbow")] IStateMachine player)
        {
            _player = player;
        }
    }
}
