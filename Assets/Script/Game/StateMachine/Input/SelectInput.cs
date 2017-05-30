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
        public SelectInput(Transform data) : base(data)
        {
        }
    }
}
