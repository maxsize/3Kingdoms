using System;
using Adic;
using Adic.Container;
using ThreeK.Game.StateMachine;
using ThreeK.Game.StateMachine.Input;
using UnityEngine;

namespace ThreeK.Game.Networking
{
    public class LocalPlayer : InjectableBehaviour
    {
        [Inject] public IInjectionContainer MainContainer;

        private IStateMachine Machine;

        protected override void Start()
        {
            base.Start();
            Machine = GetComponent<PushdownAutomation>();
        }

        // Update is called once per frame
        void Update()
        {
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
