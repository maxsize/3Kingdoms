using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adic;
using ThreeK.Game.StateMachine;

namespace ThreeK.Game.Behavior
{
    public class GameController
    {
        //private IStateMachine _player;

        public GameController(NetworkManager networkManager)
        {
            var crossbow = Resources.Load("Warrior3/Crossbow/Prefabs/Crossbow") as GameObject;
            //crossbow.AddComponent<LocalPlayer>();
            networkManager.playerPrefab = crossbow;
        }
    }
}
