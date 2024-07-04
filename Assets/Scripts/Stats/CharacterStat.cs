using RPGGame.Hero;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Stats
{
    [Serializable]
    public class CharacterStat
    {
        Dictionary<StatTypes, CharacterAttribute> _characterAttributesDict;
        public event Action OnCharacterAttributesChanged;

        public CharacterStat(CharacterBaseStats baseStats, int level)
        {
            _characterAttributesDict = new Dictionary<StatTypes, CharacterAttribute>();

            var attributes = baseStats.BaseAttributes;
            for (int i = 0; i < attributes.Length; i++)
            {
                _characterAttributesDict.Add(attributes[i].StatType.Type, new CharacterAttribute(attributes[i], level));
            }
        }

        public void AddModifier(AttributeModifier modifier)
        {
            var characterAttribute = _characterAttributesDict[modifier.StatType.Type];
            if (characterAttribute != null)
            {
                characterAttribute.AddAttributeModifier(modifier);
                OnCharacterAttributesChanged?.Invoke();
            }
        }

        public void RemoveModifier(AttributeModifier modifier)
        {
            var characterAttribute = _characterAttributesDict[modifier.StatType.Type];
            if (characterAttribute != null)
            {
                characterAttribute.RemoveAttributeModifier(modifier);
                OnCharacterAttributesChanged?.Invoke();
            }
        }

        public void HandleOnPlayerLeveldUp()
        {
            foreach (var attr in _characterAttributesDict.Values)
            {
                attr.IncreaseBaseValue();
            }

            OnCharacterAttributesChanged?.Invoke();
        }

        public float GetAttributeValue(StatTypes statType)
        {
            var characterAttribute = _characterAttributesDict[statType];
            return characterAttribute.GetAttributeValue();
        }
    }
}

