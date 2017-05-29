using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using ThreeK.Game.StateMachine;
using ThreeK.Game.StateMachine.State;
using ThreeK.Game.Behavior.Core;
using System.Collections.Generic;
using System.Linq;
using Adic;
using Assets.Script.Game.Data;
using Game.Behavior.Anim;
using Game.Behavior.Movement;
using ThreeK.Game.Behavior.Movement;
using ThreeK.Game.Helper;

namespace ThreeK.Game.Networking
{
    [InjectFromContainer(BindingHelper.Identifiers.MainContainer)]
    public class NetworkUnit : NetworkBehaviour
    {
        [Inject]
        public Metadata Meta
        {
            get;
            set;
        }

        private List<MovementBehaviour> _movements;
        private MovementBehaviour _currentMovement;
        private PushdownAutomation _stateMachine;
        private NetworkClient _client;

        private void Start()
        {
            this.Inject();
            _movements = GetComponents<MovementBehaviour>().ToList();
            for (int i = 0; i < _movements.Count; i++)
            {
                _movements[i].enabled = false;
            }

            if (isLocalPlayer)
            {
                _stateMachine = gameObject.AddComponent<Player>();
                _stateMachine.OnStateChange.AddListener(OnStateChange);
                gameObject.AddComponent<LocalPlayer>();
                GetComponent<CollisionDetector>().enabled = true;
            }
            else
            {
                gameObject.layer = LayerMask.NameToLayer("Enemy");
            }
            if (IsHost)
                NetworkServer.RegisterHandler(NetworkUnitMessage.MessageType, OnClientToServer);
            else if (isClient)
            {
                NetworkManager manager = FindObjectOfType<NetworkManager>();
                _client = new NetworkClient();
                _client.RegisterHandler(MsgType.Connect, OnConnect);
                _client.RegisterHandler(NetworkUnitMessage.MessageType, OnMessageReceive);
                _client.Connect(manager.networkAddress, manager.networkPort);
            }
        }

        private void OnStateChange(IState newState)
        {
            StartMovement(newState);
        }

        private void StartMovement(IState state)
        {
            var data = MovementHelper.GetMovementData(state);
            StartMovement(data);
        }

        private void StartMovement(MovementData data, float latency = 0.0f)
        {
            if (_currentMovement != null)
            {
                _currentMovement.End();
                _currentMovement.OnEnd.RemoveListener(OnStateEnd);
                //_currentMovement.enabled = false;
            }

            var m = GetMovement(data.MovementType);
            if (m != null)
            {
                m.OnEnd.AddListener(OnStateEnd);
                m.SetTarget(data.Data, latency);
                _currentMovement = m;
                Sync(data);
            }
        }

        private void Sync(MovementData data)
        {
            // Only local player should send message (clients should not send it)
            if (!isLocalPlayer)
                return;
            var msg = new NetworkUnitMessage()
            {
                Position = transform.position,
                NetId = netId
            };
            msg.SetData(data);
            SendMessage(msg, NetworkTransport.GetNetworkTimestamp());
        }

        void SendMessage(NetworkUnitMessage msg, int timestamp)
        {
            msg.Timestamp = timestamp;     // Add timestamp
            if (IsHost)
                NetworkServer.SendToAll(NetworkUnitMessage.MessageType, msg);
            else
            {
                if (_client.connection != null)
                    _client.Send(NetworkUnitMessage.MessageType, msg);
            }
        }

        private void OnStateEnd()
        {
            _currentMovement.OnEnd.RemoveListener(OnStateEnd);
            if (isLocalPlayer)
                _stateMachine.NextState();
        }

        private MovementBehaviour GetMovement(Type mType)
        {
            var found = _movements.Find(m => m.GetType() == mType);
            if (found == null)
            {
                found = gameObject.AddComponent(mType)  as MovementBehaviour;
                _movements.Add(found);
            }
            return found;
        }

        private bool IsHost
        {
            get { return isServer && isClient; }
        }

        private void OnClientToServer(NetworkMessage netMsg)
        {
            // Sender on server side will receive this message
            // When server receives messages, sync movement and send to all the clients
            var msg = netMsg.ReadMessage<NetworkUnitMessage>();
            var latency = GetLatency(netMsg.conn, msg.Timestamp, "Client to Server");
            SyncClientObject(msg, latency);
            var timestamp = NetworkTransport.GetNetworkTimestamp() - latency;   // Add current message delay and send to everyone
            SendMessage(msg, timestamp);
        }

        private void SyncClientObject(NetworkUnitMessage msg, float latency)
        {
            var clientObj = isServer ?
                NetworkServer.FindLocalObject(msg.NetId) :
                ClientScene.FindLocalObject(msg.NetId);
            if (clientObj == null)
            {
                Debug.LogError(string.Format("Client {0} not found!", msg.NetId));
                return;
            }
            clientObj.transform.position = msg.Position;
            var unit = clientObj.GetComponent<NetworkUnit>();
            if (unit.isLocalPlayer) return;     // This is the sender it self

            var data = msg.GetData(Meta);
            if (data.Data is NetworkInstanceId)
            {
                // In this case, will need to get client transform
                var go = ClientScene.FindLocalObject((NetworkInstanceId)data.Data);
                if (go == null)
                {
                    throw new Exception(string.Format("network instance {0} not found.", data));
                }
                data.Data = go.transform;   // Override instanceId as client transform
            }
            unit.StartMovement(data, latency);
        }

        private void OnConnect(NetworkMessage netMsg)
        {
        }

        private void OnMessageReceive(NetworkMessage netMsg)
        {
            // When client receives messages, just sync movement and no further message will be sent
            var msg = netMsg.ReadMessage<NetworkUnitMessage>();
            var latency = GetLatency(netMsg.conn, msg.Timestamp, "Server to Client");
            //if (isServer) return;
            if (msg.NetId == netId) return;     // Don't sync the sender it self
            SyncClientObject(msg, latency);
        }

        private int GetLatency(NetworkConnection conn, int remoteTimestamp, string debugMsg)
        {
            byte error;
            var delay = NetworkTransport.GetRemoteDelayTimeMS(conn.hostId, conn.connectionId, remoteTimestamp, out error);
            Debug.Log(debugMsg + " Latency: " + delay);
            return delay / 1000;
        }
    }

    public class NetworkUnitMessage : MessageBase
    {
        public static short MessageType = MsgType.Highest + 1;

        public string MovementType;
        public NetworkInstanceId NetId;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Velocity;
        public NetworkInstanceId Target;
        public string AbilityName;
        public int Timestamp;

        public void SetData(MovementData data)
        {
            MovementType = data.MovementType.Name;
            if (data.MovementType == typeof(Mover))
            {
                Velocity = (Vector3)data.Data;
            }
            else if (data.MovementType == typeof(Spinner))
            {
                Rotation = (Quaternion)data.Data;
            }
            else if (data.MovementType == typeof(Mover2) ||
                data.MovementType == typeof(CastingMover) ||
                data.MovementType == typeof(Attacker))
            {
                Target = ((Transform)data.Data).GetComponent<NetworkIdentity>().netId;
            }

            if (data.Ability.Name != null)
            {
                // Casting ability, sync ability info
                AbilityName = data.Ability.Name;
                if (data.Ability.IsUnitTarget())
                    Target = ((Transform)data.Data).GetComponent<NetworkIdentity>().netId;
                if (data.Ability.IsPointTarget())
                    Position = (Vector3) data.Data;
            }
        }

        public MovementData GetData(Metadata meta)
        {
            object value = null;
            AbilityVO ability = default(AbilityVO);
            switch (MovementType)
            {
                case "Attacker":
                case "Mover2":
                case "CastingMover":
                    value = Target;
                    break;
                case "Mover":
                    value = Velocity;
                    break;
                case "Spinner":
                    value = Rotation;
                    break;
                default:
                    ability = meta.Abilities.Find(a => a.Name == AbilityName);
                    if (ability.Name != null)
                    {
                        if (ability.IsUnitTarget())
                            value = Target;
                        if (ability.IsPointTarget())
                            value = Position;
                    }
                    break;
            }
            var data = new MovementData()
            {
                MovementType = string.IsNullOrEmpty(AbilityName) ? 
                Type.GetType("ThreeK.Game.Behavior.Movement." + MovementType) :
                Type.GetType("ThreeK.Game.Behavior.Movement.Cast." + MovementType),
                Data = value,
                Ability = ability
            };
            return data;
        }
    }
}
