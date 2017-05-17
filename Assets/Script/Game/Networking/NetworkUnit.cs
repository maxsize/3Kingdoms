﻿using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System;
using ThreeK.Game.StateMachine;
using ThreeK.Game.StateMachine.State;
using ThreeK.Game.Behavior.Core;
using System.Collections.Generic;
using System.Linq;
using ThreeK.Game.Behavior.Movement;

namespace ThreeK.Game.Networking
{
    public class NetworkUnit : NetworkBehaviour
    {
        private List<MovementBehaviour> _movements;
        private MovementBehaviour _currentMovement;
        private PushdownAutomation _stateMachine;
        private NetworkClient _client;

        private void Start()
        {
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
            }
            if (IsHost)
                NetworkServer.RegisterHandler(NetworkUnitMessage.MessageType, OnClientToServer);
            else if (isClient)
            {
                _client = new NetworkClient();
                _client.RegisterHandler(MsgType.Connect, OnConnect);
                _client.RegisterHandler(NetworkUnitMessage.MessageType, OnAnim);
                _client.Connect("localhost", 7777);
            }
        }

        private void OnStateChange(IState newState)
        {
            var data = newState.Data;
            StartMovement(data);
        }

        private void StartMovement(object data, float latency = 0.0f)
        {
            if (_currentMovement != null)
                _currentMovement.OnEnd.RemoveListener(OnStateEnd);

            MovementBehaviour m = null;
            if (data is Quaternion)
                m = GetMovement(typeof(Spinner));
            else if (data is Vector3)
                m = GetMovement(typeof(Mover));
            else if (data is Transform)
            {
                var t = data as Transform;
                if (Vector3.Distance(t.position, transform.position) <= 2)
                    m = GetMovement(typeof(Attacker));
                else
                    m = GetMovement(typeof(Mover2));
            }
            else if (data == null)
                m = GetMovement(typeof(Stander));

            if (m != null)
            {
                m.OnEnd.AddListener(OnStateEnd);
                m.SetTarget(data, latency);
                _currentMovement = m;
                Sync(data);
            }
        }

        private void Sync(object data)
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
            SendMessage(msg);
        }

        void SendMessage(NetworkUnitMessage msg)
        {
            msg.Timestamp = NetworkTransport.GetNetworkTimestamp();     // Add timestamp
            if (IsHost)
                NetworkServer.SendToAll(NetworkUnitMessage.MessageType, msg);
            else
                _client.Send(NetworkUnitMessage.MessageType, msg);
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
            var latency = GetLatency(netMsg.conn, msg.Timestamp);
            SyncClientObject(msg, latency);
            msg.Timestamp -= latency;   // Add current message delay and send to everyone
            SendMessage(msg);
            //var keys = NetworkServer.objects;
            //foreach (KeyValuePair<NetworkInstanceId, NetworkIdentity> pair in keys)
            //{
            //    if (msg.NetId == pair.Key) continue;
            //    NetworkServer.SendToClient(pair.Value.connectionToClient.connectionId, NetworkUnitMessage.MessageType, msg);
            //}
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
            var unit = clientObj.GetComponent<NetworkUnit>();
            if (unit.isLocalPlayer) return;     // This is the sender it self
            unit.StartMovement(msg.GetData(), latency);
        }

        private void OnConnect(NetworkMessage netMsg)
        {
        }

        private void OnAnim(NetworkMessage netMsg)
        {
            // When client receives messages, just sync movement and no further message will be sent
            var msg = netMsg.ReadMessage<NetworkUnitMessage>();
            var latency = GetLatency(netMsg.conn, msg.Timestamp);
            //if (isServer) return;
            if (msg.NetId == netId) return;     // Don't sync the sender it self
            SyncClientObject(msg, latency);
        }

        private int GetLatency(NetworkConnection conn, int remoteTimestamp)
        {
            byte error;
            var delay = NetworkTransport.GetRemoteDelayTimeMS(conn.hostId, conn.connectionId, remoteTimestamp, out error);
            Debug.Log("Latency: " + delay);
            return delay / 1000;
        }
    }

    public class NetworkUnitMessage : MessageBase
    {
        public static short MessageType = MsgType.Highest + 1;

        public enum SyncType
        {
            Rotate,
            Move,
            Follow,
            Attack,
            Idle
        }

        public SyncType Type;
        public NetworkInstanceId NetId;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Velocity;
        public NetworkInstanceId Target;
        public int Timestamp;

        public void SetData(object data)
        {
            if (data is Vector3)
            {
                Velocity = (Vector3)data;
                Type = SyncType.Move;
            }
            else if (data is Quaternion)
            {
                Rotation = (Quaternion)data;
                Type = SyncType.Rotate;
            }
            else if (data is NetworkInstanceId)
            {
                Target = (NetworkInstanceId)data;
                Type = SyncType.Follow;
            }
            else if (data == null)
            {
                Type = SyncType.Idle;
            }
        }

        public object GetData()
        {
            object value = null;
            switch (Type)
            {
                case SyncType.Attack:
                case SyncType.Follow:
                    value = Target;
                    break;
                case SyncType.Move:
                    value = Velocity;
                    break;
                case SyncType.Rotate:
                    value = Rotation;
                    break;
            }
            return value;
        }
    }
}