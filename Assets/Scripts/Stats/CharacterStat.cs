using RPGGame.Hero;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Stats
{
    //[CreateAssetMenu(menuName ="ScriptableObjects/Stats/CharacterStats")]
    [Serializable]
    public class CharacterStat
    {
        public List<CharacterAttribute> _characterAttributes;
        //public List<CharacterAttribute> CharacterAttributes { get => _characterAttributes; }

        public event Action OnCharacterAttributesChanged;

        public CharacterStat(CharacterBaseStats baseStats, int level)
        {
            _characterAttributes = new List<CharacterAttribute>();

            var attributes = baseStats.BaseAttributes;
            for (int i = 0; i < attributes.Length; i++)
            {
                _characterAttributes.Add(new CharacterAttribute(attributes[i], level));
            }
        }

        public void AddModifier(AttributeModifier modifier)
        {
            var characterAttribute = FindAttribute(modifier.StatType);
            if (characterAttribute != null)
            {
                characterAttribute.AddAttributeModifier(modifier);
                OnCharacterAttributesChanged?.Invoke();
            }
        }

        public void RemoveModifier(AttributeModifier modifier)
        {
            var characterAttribute = FindAttribute(modifier.StatType);
            if (characterAttribute != null)
            {
                characterAttribute.RemoveAttributeModifier(modifier);
                OnCharacterAttributesChanged?.Invoke();
            }
        }

        public void HandleOnPlayerLeveldUp()
        {
            for (int i = 0; i < _characterAttributes.Count; i++)
            {
                _characterAttributes[i].IncreaseBaseValue();
            }
            OnCharacterAttributesChanged?.Invoke();
        }

        public CharacterAttribute FindAttribute(StatType statType)
        { 
            for (int i = 0; i < _characterAttributes.Count; i++)
            {
                if (_characterAttributes[i].StatType.ID== statType.ID)
                {
                    return _characterAttributes[i];
                }
            }

            return null;
        }

        public float GetAttributeValue(StatType statType)
        {
            var attr = FindAttribute(statType);
            return attr.GetAttributeValue();
        }

        public float CalculateAttributeValue(StatType statType)
        {
            var attr = FindAttribute(statType);
            return attr.CalculateAttributeValue();
        }
    }
}

