using System;
using Adic;
using Adic.Container;
using ThreeK.Game.Helper;
using ThreeK.Game.StateMachine;
using ThreeK.Game.StateMachine.Input;
using UnityEngine;

namespace ThreeK.Game.Networking
{
    public class LocalPlayer : InjectableBehaviour
    {
        [Inject] public IInjectionContainer MainContainer;

        private IStateMachine _machine;

        protected override void Start()
        {
            base.Start();
            _machine = GetComponent<PushdownAutomation>();
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
            IInput input = null;
            //if (inputType == typeof(AttackInput))
            //{
            //    AttackInput attackInput = MainContainer.Resolve<AttackInput>();
            //    input = attackInput;
            //}
            //else if (inputType == typeof(MoveInput))
            //{
            //    MoveInput moveInput = MainContainer.Resolve<MoveInput>();
            //    input = moveInput;
            //}

            input = DynamicInvoke.Invoke(MainContainer, inputType, "Resolve") as IInput;
            
            if (input != null)
                _machine.HandleInput(input);
            return input != null;
        }
    }
}
