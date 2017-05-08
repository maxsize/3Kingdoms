using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace ThreeK.Game.StateMachine.Input
{
    public interface IInputResolver
    {
        void Register(int code, IInput input);
        IInput Resolve(int code);
    }

    public class InputResolver : IInputResolver
    {
        private Dictionary<int, IInput> dic;

        public InputResolver(Dictionary<int, IInput> dictionary = null)
        {
            dic = dictionary == null ? new Dictionary<int, IInput>() : dictionary;
        }

        public void Register(int code, IInput input)
        {
            if (dic.ContainsKey(code))
                throw new Exception(string.Format("Input code {0} already exist.", code));
            dic.Add(code, input);
        }

        public IInput Resolve(int code)
        {
            IInput input;
            if (!dic.TryGetValue(code, out input))
                throw new Exception(string.Format("Input code {0} not registered.", code));
            return input;
        }
    }
}
