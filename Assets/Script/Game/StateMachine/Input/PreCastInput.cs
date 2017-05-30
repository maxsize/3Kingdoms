using Assets.Script.Game.Data;
using UnityEngine;

namespace ThreeK.Game.StateMachine.Input
{
    /// <summary>
    /// When player choose a unit or point type of ability, this input will be created
    /// </summary>
    public class PreCastInput : GameInput
    {
        public PreCastInput(AbilityVO ability) : base(ability)
        {
        }
    }
}
