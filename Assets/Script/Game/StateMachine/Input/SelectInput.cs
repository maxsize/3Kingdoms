using Assets.Script.Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ThreeK.Game.StateMachine.Input
{
    /// <summary>
    /// Will be create when left click on any item on the map
    /// </summary>
    public class SelectInput : GameInput
    {
        public AbilityVO Ability { get; private set; }

        public SelectInput(Transform data, AbilityVO ability) : base(data)
        {
            Ability = ability;
        }
    }
}
