using System;
using System.Reflection;
using Adic;
using Adic.Container;
using Assets.Script.Game.Data;
using ThreeK.Game.Helper;
using ThreeK.Game.StateMachine;
using ThreeK.Game.StateMachine.Input;
using UnityEngine;
using System.Linq;
using ThreeK.Game.Data;

namespace ThreeK.Game.Networking
{
    public class LocalPlayer : InjectableBehaviour
    {
        [Inject] public IInjectionContainer MainContainer;
        [Inject] public Metadata Meta;
        [Inject] public PlayerVO Player;

        private IStateMachine _machine;

        protected override void Start()
        {
            base.Start();
            _machine = GetComponent<PushdownAutomation>();
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                if (CreateAndHandleInput(typeof(AttackInput))) return;
                if (CreateAndHandleInput(typeof(MoveInput))) return;
            }
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.W))
            {
                var ability = Meta.Abilities.ToList().Find(a => a.Name == Player.CastingAbility);
                if (ability.IsNoTarget())
                    CreateAndHandleInput(typeof(CastInput));
                else
                    CreateAndHandleInput(typeof(PreCastInput));
            }
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (CreateAndHandleInput(typeof(SelectInput))) return;
                if (CreateAndHandleInput(typeof(PointInput))) return;
            }
        }

        private bool CreateAndHandleInput(Type inputType, object identifier = null)
        {
            IInput input = identifier != null ?
                MainContainer.Resolve(inputType, identifier) as IInput :
                MainContainer.Resolve(inputType) as IInput;

            if (input != null)
                _machine.HandleInput(input);
            return input != null;
        }
    }
}
