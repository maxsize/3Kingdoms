using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ThreeK.Game.StateMachine.Input;
using ThreeK.Game.StateMachine.State;
using UnityEngine;

namespace _3Kingdoms.Game.StateMachine.State
{
    public abstract class MonoState : ThreeK.Game.StateMachine.State.State
    {
        protected MonoBehaviour Machine;

        protected MonoState(MonoBehaviour machine)
        {
            Machine = machine;
        }

        protected void StartCoroutine(IEnumerator routine)
        {
            Machine.StartCoroutine(routine);
        }
    }
}
