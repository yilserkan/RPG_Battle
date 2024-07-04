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
    }

    [Serializable]
    public struct CharacterBaseAttribute
    {
        public StatTypeSettings StatType;
        public float BaseValue;
    }
}
