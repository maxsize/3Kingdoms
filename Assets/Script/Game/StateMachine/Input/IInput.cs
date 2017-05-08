using UnityEngine;
using System.Collections;

namespace ThreeK.Game.StateMachine.Input
{
    public interface IInput
    {
        string Name { get; }
        int Code { get; }
        object Data { get; }
    }
}
