using UnityEngine;
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
                NetworkServer.RegisterHandler(MsgType.Animation, OnClientToServer);
            else if (isClient)
            {
                _client = new NetworkClient();
                _client.RegisterHandler(MsgType.Connect, OnConnect);
                _client.RegisterHandler(MsgType.Animation, onAnim);
                _client.Connect("localhost", 7777);
            }
        }

        private void OnStateChange(IState newState)
        {
            var data = newState.Data;
            StartMovement(data);
        }

        private void StartMovement(object data)
        {
            if (_currentMovement != null)
                _currentMovement.OnEnd.RemoveListener(OnStateEnd);

            MovementBehaviour m = null;
            if (data is Quaternion)
                m = GetMovement(typeof(Spinner));
            if (data is Vector3)
                m = GetMovement(typeof(Mover));
            if (data is Transform)
            {
                var t = data as Transform;
                if (Vector3.Distance(t.position, transform.position) <= 2)
                    m = GetMovement(typeof(Attacker));
                else
                    m = GetMovement(typeof(Mover2));
            }
            if (m != null)
            {
                m.OnEnd.AddListener(OnStateEnd);
                m.SetTarget(data);
                _currentMovement = m;
                Sync(data);
            }
        }

        private void Sync(object data)
        {
            var msg = new NetworkUnitMessage()
            {
                Position = transform.position,
                NetId = netId
            };
            if (data is Vector3)
            {
                msg.Velocity = (Vector3)data;
                msg.Type = NetworkUnitMessage.SyncType.Move;
            }
            else if (data is Quaternion)
            {
                msg.Rotation = (Quaternion)data;
                msg.Type = NetworkUnitMessage.SyncType.Rotate;
            }
            else if (data is NetworkInstanceId)
            {
                msg.Target = (NetworkInstanceId)data;
                msg.Type = NetworkUnitMessage.SyncType.Follow;
            }
            SendMessage(msg);
        }

        void SendMessage(MessageBase msg)
        {
            if (IsHost)
                NetworkServer.SendToAll(MsgType.Animation, msg);
            else
                _client.Send(MsgType.Animation, msg);
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
            // When server receives messages, sync movement and send to all the clients
            var msg = netMsg.ReadMessage<NetworkUnitMessage>();
            SyncClientObject(msg);
            NetworkServer.SendToAll(MsgType.Animation, msg);
        }

        private void SyncClientObject(NetworkUnitMessage msg)
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
            unit.StartMovement(msg.GetData());
        }

        private void OnConnect(NetworkMessage netMsg)
        {
        }

        private void onAnim(NetworkMessage netMsg)
        {
            // When client receives messages, just sync movement and no further message will be sent
            var msg = netMsg.ReadMessage<NetworkUnitMessage>();
            if (msg.NetId == netId) return;     // Don't sync self
            SyncClientObject(msg);
        }
    }

    public class NetworkUnitMessage : MessageBase
    {
        public enum SyncType
        {
            Rotate,
            Move,
            Follow,
            Attack
        }

        public SyncType Type;
        public NetworkInstanceId NetId;
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Velocity;
        public NetworkInstanceId Target;

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
