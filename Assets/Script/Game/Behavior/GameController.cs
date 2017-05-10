using UnityEngine.Networking;
using UnityEngine;

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
