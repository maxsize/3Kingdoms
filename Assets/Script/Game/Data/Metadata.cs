using System;
using System.Collections.Generic;

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
    public class Metadata
    {
        public List<AbilityVO> Abilities;
    }

    [Serializable]
    public struct AbilitiesVO
    {
        public List<AbilityVO> Abilities;
    }

    [Serializable]
    public struct AbilityVO
    {
        public string Name;
        public float Radius;
        public float CastRange;
        public List<int> AbilityTypes;
        public List<int> Effects;
        public List<AbilityLevelVO> Levels;

        public bool ToAlly()
        {
            return Effects != null && Effects.Contains(1);
        }

        public bool ToEnemy()
        {
            return Effects != null && Effects.Contains(0);
        }

        public bool IsNull()
        {
            return string.IsNullOrEmpty(Name);
        }

        public bool IsNoTarget()
        {
            return AbilityTypes != null && AbilityTypes.Contains(0);
        }

        public bool IsUnitTarget()
        {
            return AbilityTypes != null && AbilityTypes.Contains(1);
        }

        public bool IsPointTarget()
        {
            return AbilityTypes != null && AbilityTypes.Contains(2);
        }
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
