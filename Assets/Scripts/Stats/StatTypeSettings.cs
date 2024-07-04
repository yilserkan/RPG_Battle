
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Stats
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Stats/TypeSettings")]
    public class StatTypeSettings: ScriptableObject
    {
        public StatTypes Type;
        public string ID;
        public string Name;
        [Range(0, 100)] public float LevelUpMultiplier = 10;
    }

    public enum StatTypes
    {
        Attack,
        Vitality
    }
}

