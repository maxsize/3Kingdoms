using System;

namespace Assets.Script.Game.Data
{
    public enum AbilityTypes
    {
        NoTarget = 0,
        UnitTarget = 1,
        PointTarget = 2
    }

    public enum Effects
    {
        Enemy = 0,
        Ally = 1
    }

    [Serializable]
    public struct Metadata
    {
        public AbilityVO[] Abilities;
    }

    [Serializable]
    public struct AbilityVO
    {
        public string Name;
        public float Radius;
        public AbilityTypes[] AbilityTypes;
        public Effects[] Effects;
        public AbilityLevelVO[] Levels;
    }

    [Serializable]
    public struct AbilityLevelVO
    {
        public int Index;
        public int Damage;
        public int MaxDamage;
        public int ManaCost;
        public int Cooldown;
        public int Duration;
    }
}
