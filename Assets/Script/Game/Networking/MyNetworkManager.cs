using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using Adic;
using ThreeK.Game.Data;
using Adic.Container;
using ThreeK.Game.Event;
using System;

namespace ThreeK.Game.Networking
{

    public class MyNetworkManager : NetworkManager
    {
        [Inject] public EventDispatcher Dispatcher;
        [Inject] public PlayerVO PlayerData;

        private void Start()
        {
            this.Inject();
            Dispatcher.AddListener<ClientConnectEvent>(OnConnect);
        }

        /// <summary>
        /// Called on server side after client called <code>ClientScene.AddPlayer</code>
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="playerControllerId"></param>
        /// <param name="extraMessageReader"></param>
        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId, NetworkReader extraMessageReader)
        {
            var msg = extraMessageReader.ReadMessage<PlayerConnectMessage>();
            if (msg == null)
                return;
            var pref = spawnPrefabs.Find(p => p.name == msg.HeroName);
            var player = Instantiate(pref);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }

        /// <summary>
        /// Called on client when connected to server
        /// </summary>
        /// <param name="conn"></param>
        public override void OnClientConnect(NetworkConnection conn)
        {
            var msg = new PlayerConnectMessage()
            {
                HeroName = PlayerData.HeroName
            };
            Debug.Log("Client connected " + conn.address);
            // By calling this method will trigger OnServerAddPlayer on server side
            ClientScene.AddPlayer(conn, 0, msg);
        }

        private void OnConnect(ClientConnectEvent e)
        {
            var client = e.Data as NetworkClient;
            NetworkMessageDelegate OnClient = delegate (NetworkMessage msg)
            {
                Debug.Log(msg);
                client.UnregisterHandler(MsgType.Connect);
            };
            client.RegisterHandler(MsgType.Connect, OnClient);
        }
    }

    public class PlayerConnectMessage : MessageBase
    {
        public string HeroName;
    }
}
