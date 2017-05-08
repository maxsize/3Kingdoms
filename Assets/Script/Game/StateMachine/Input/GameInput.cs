using UnityEngine;
using System.Collections;
using System;

namespace ThreeK.Game.StateMachine.Input
{
    public class GameInput : IInput
    {
        private string _name;
        private int _code = -1;
        private object _data;

        public GameInput(object data)
        {
            _data = data;
        }

        public string Name
        {
            get
            {
                if (_name == null)
                    _name = GetType().Name;
                return _name;
            }
        }

        public int Code
        {
            get
            {
                if (_code == -1)
                    _code = Name.GetHashCode();
                return _code;
            }
        }

        public object Data
        {
            get { return _data; }
        }
    }
}
