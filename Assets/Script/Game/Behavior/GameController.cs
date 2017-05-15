using Adic;
using Adic.Container;
using ThreeK.Game.Networking;
using UnityEngine;
using UnityEngine.Networking;

namespace ThreeK.Game.Behavior
{
    public class GameController
    {
        [Inject] public IInjectionContainer Container;

        public GameController(NetworkManager networkManager)
        {
            var crossbow = Resources.Load("Warrior3/Crossbow/Prefabs/Crossbow") as GameObject;
            //crossbow.AddComponent<LocalPlayer>();
            networkManager.playerPrefab = crossbow;
        }
    }
}
