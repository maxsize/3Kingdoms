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
        }
    }
}
