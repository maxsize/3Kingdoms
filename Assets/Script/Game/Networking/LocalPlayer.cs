using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Adic;
using Adic.Container;
using ThreeK.Game.StateMachine;
using ThreeK.Game.StateMachine.Input;
using UnityEngine;
using UnityEngine.Networking;

namespace ThreeK.Game.Networking
{
    public class LocalPlayer : NetworkBehaviour
    {
        [Inject] public IInjectionContainer MainContainer;
        [Inject] public IStateMachine Machine;

        void Start()
        {
            this.Inject();
        }

        // Update is called once per frame
        void Update()
        {
            if (!isLocalPlayer)
                return;
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (CreateAndHandleInput(typeof(AttackInput))) return;
                if (CreateAndHandleInput(typeof(MoveInput))) return;
            }
        }

        private bool CreateAndHandleInput(Type inputType)
        {
            var input = MainContainer.Resolve<IInput>(inputType);
            if (input != null)
                Machine.HandleInput(input);
            return input != null;
        }
    }
}
