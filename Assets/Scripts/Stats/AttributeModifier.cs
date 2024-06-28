using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Stats
{
    [Serializable]
    public class AttributeModifier
    {
        [SerializeField] private float _value;
        [SerializeField] private AttributeModifierType _type;
        [SerializeField] private StatType _statType;

        public float Value { get => _value; }
        public AttributeModifierType Type { get => _type; }
        public StatType StatType { get => _statType; }

        public AttributeModifier(float value, StatType statType, AttributeModifierType attributeModifierType = AttributeModifierType.Additive)
        {
            _value = value;
            _type = attributeModifierType;
            _statType = statType;
        }

        public string GetText()
        {
            string attrType = _type == AttributeModifierType.Additive ? "+" : "%";
            return $"{_statType.Name} {attrType} {_value}";
        }
    }

    public enum AttributeModifierType
    {
        Additive,
        Percent
    }
}
