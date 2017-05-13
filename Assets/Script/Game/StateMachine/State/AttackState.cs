using UnityEngine;
using System.Collections;
using System;
using Adic;
using ThreeK.Game.StateMachine.Input;
using ThreeK.Game.Helper;

namespace ThreeK.Game.StateMachine.State
{
    public class AttackState : State
    {
        private const string ATTACK1_TRIGGER = "Attack1Trigger";

        private Transform _data;

        public AttackState()
        {
            _data = (Transform)InputHelper.CurrentInput.Data;
        }

        public override object Data
        {
            get { return _data; }
        }

        public override IState HandleInput(IInput input)
        {
            return this;
        }
    }
}
