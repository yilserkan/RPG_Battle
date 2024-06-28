using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Stats
{
    [CreateAssetMenu(menuName ="ScriptableObjects/Stats/BaseAttributes")]
    public class CharacterBaseStats : ScriptableObject
    {
        public CharacterBaseAttribute[] BaseAttributes;
        public string HeroID;
    }

    [Serializable]
    public struct CharacterBaseAttribute
    {
        public StatType StatType;
        public float BaseValue;
    }
}
