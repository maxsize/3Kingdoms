using UnityEngine;
using System.Collections;

namespace ThreeK.Game.StateMachine
{
    public interface IMonoStateMachine : IStateMachine
    {
        MonoBehaviour Client { get; }
    }
}
