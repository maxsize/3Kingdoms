using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ThreeK.Game.StateMachine.Input
{
    /// <summary>
    /// Will be create when left click on ground
    /// </summary>
    public class PointInput : GameInput
    {
        public PointInput(Vector3 data) : base(data)
        {
        }
    }
}
