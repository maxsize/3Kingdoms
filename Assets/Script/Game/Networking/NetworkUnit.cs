using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

namespace ThreeK.Game.Networking
{
    [RequireComponent(typeof(NetworkTransform))]
    public class NetworkUnit : InjectableBehaviour
    {
        private NetworkManager networkManager;
        private NetworkIdentity networkId;

        protected override void Start()
        {
            base.Start();
            networkId = GetComponent<NetworkIdentity>();
            if (networkId.isLocalPlayer)
            {
                gameObject.AddComponent<Player>();
                gameObject.AddComponent<LocalPlayer>();
            }
        }
    }
}
