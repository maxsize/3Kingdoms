using UnityEngine;
using System.Collections;

namespace ThreeK.Game.Data
{
    public class PlayerVO
    {
        public string HeroName;
        public string[] Abilities;

        public string CastingAbility
        {
            get
            {
                int index = -1;
                if (Input.GetKeyDown(KeyCode.Q)) index = 0;
                if (Input.GetKeyDown(KeyCode.W)) index = 1;
                return index >= 0 ? Abilities[index] : null;
            }
        }
    }
}
