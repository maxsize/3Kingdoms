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

        private void Start()
        {
            _movements = GetComponents<MovementBehaviour>().ToList();

            if (isLocalPlayer)
            {
                _stateMachine = gameObject.AddComponent<Player>();
                _stateMachine.OnStateChange.AddListener(OnStateChange);
                gameObject.AddComponent<LocalPlayer>();
            }
            if (IsHost)
                NetworkServer.RegisterHandler(MsgType.Connect, OnConnect);
            else if (isClient)
            {
                var client = new NetworkClient();
                client.RegisterHandler(MsgType.Connect, OnConnect);
                client.RegisterHandler(MsgType.Animation, onAnim);
                client.Connect("localhost", 7777);
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
            }
        }

        private void OnStateEnd()
        {
            _currentMovement.OnEnd.RemoveListener(OnStateEnd);
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

        private void OnConnect(NetworkMessage netMsg)
        {
        }

        private void onAnim(NetworkMessage netMsg)
        {
        }
    }

    public class NetworkUnitMessage : MessageBase
    {
        public NetworkInstanceId NetId;
        public Quaternion Rotation;
        public Vector3 Velocity;
    }
}
