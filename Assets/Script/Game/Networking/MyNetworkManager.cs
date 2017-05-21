using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Adic;
using ThreeK.Game.Data;
using Adic.Container;

namespace ThreeK.Game.Networking
{

    public class MyNetworkManager : NetworkManager
    {
        [Inject] public PlayerVO PlayerData;

        private void Start()
        {
            this.Inject();
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
        {
            var msg = extraMessageReader.ReadMessage<PlayerConnectMessage>();
            var pref = spawnPrefabs.Find(p => p.name == msg.HeroName);
            var player = Instantiate(pref);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            var msg = new PlayerConnectMessage()
            {
                HeroName = PlayerData.HeroName
            };
            ClientScene.AddPlayer(conn, 0, msg);
        }
    }

    public class PlayerConnectMessage : MessageBase
    {
        public string HeroName;
    }
}
