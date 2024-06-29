using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Stats
{
    [Serializable]
    public class CharacterAttribute
    {
        [SerializeField] public StatType StatType;
        [SerializeField] public float BaseValue;

        private List<AttributeModifier> _additiveModifiers = new List<AttributeModifier>();
        private List<AttributeModifier> _percentModifiers = new List<AttributeModifier>();

        private float _attributeValue;

        public CharacterAttribute(CharacterBaseAttribute characterBaseAttribute, int level)
        {
            StatType = characterBaseAttribute.StatType;
            BaseValue = CalculateBaseValueFromLevel(characterBaseAttribute.BaseValue, level);
        }

        private float CalculateBaseValueFromLevel(float baseValue, int level)
        {
            var multiplierPercentage = (100 + StatType.LevelUpMultiplier) / 100;
            var mulitplier = Mathf.Pow(multiplierPercentage, level-1);
            return mulitplier * baseValue;
        }

        public void AddAttributeModifier(AttributeModifier attributeModifier)
        {
            if (attributeModifier == null) { return; }
            if (attributeModifier.Type == AttributeModifierType.Additive)
            {
                _additiveModifiers.Add(attributeModifier);
                CalculateAttributeValue();
            }
            else
            {
                _percentModifiers.Add(attributeModifier);
                CalculateAttributeValue();
            }
        }

        public void RemoveAttributeModifier(AttributeModifier attributeModifier) 
        {  
            if (attributeModifier == null) { return;} 
        
            if(attributeModifier.Type == AttributeModifierType.Additive && _additiveModifiers.Contains(attributeModifier))
            {
                _additiveModifiers.Remove(attributeModifier);
                CalculateAttributeValue();
            }
            else if(attributeModifier.Type == AttributeModifierType.Percent && _percentModifiers.Contains(attributeModifier)) 
            { 
                _percentModifiers.Remove(attributeModifier);
                CalculateAttributeValue();
            }
        }

        public void IncreaseBaseValue()
        {
            BaseValue *= (100 + StatType.LevelUpMultiplier) / 100;
            CalculateAttributeValue();
        }

        public float CalculateAttributeValue()
        {
            _attributeValue = BaseValue;
            ApplyAdditiveModifiers();
            ApplyPercentModifiers();
            return _attributeValue;
        }

        public float GetAttributeValue()
        {
            return _attributeValue;
        }

        private void ApplyAdditiveModifiers()
        {
            for (int i = 0; i < _additiveModifiers.Count; i++)
            {
                _attributeValue += _additiveModifiers[i].Value;
            }
        }

        private void ApplyPercentModifiers()
        {
            for (int i = 0; i < _percentModifiers.Count; i++)
            {
                _attributeValue *= (100 + _percentModifiers[i].Value) / 100;
            }
        }
    }
}
