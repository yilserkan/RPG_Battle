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
        public Action<AttributeModifiedData[]> OnCharacterAttributesChanged;

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
                var modifiedAmount = characterAttribute.AddAttributeModifier(modifier);
                var attrModifyData = new AttributeModifiedData[1];
                attrModifyData[0] =  new AttributeModifiedData(){Type = characterAttribute.StatType.Type, IncreaseAmount = modifiedAmount };
                OnCharacterAttributesChanged?.Invoke(attrModifyData);
            }
        }

        public void RemoveModifier(AttributeModifier modifier)
        {
            var levelUpdatas = new AttributeModifiedData[1];
            var characterAttribute = _characterAttributesDict[modifier.StatType.Type];
            if (characterAttribute != null)
            {
                var modifiedAmount = characterAttribute.RemoveAttributeModifier(modifier);
                var attrModifyData = new AttributeModifiedData[1];
                attrModifyData[0] = new AttributeModifiedData() { Type = characterAttribute.StatType.Type, IncreaseAmount = modifiedAmount };
                OnCharacterAttributesChanged?.Invoke(attrModifyData);
            }
        }

        public AttributeModifiedData[] HandleOnPlayerLeveldUp()
        {
            var levelUpdatas = new AttributeModifiedData[_characterAttributesDict.Values.Count];
            int i = 0;
            foreach (var attr in _characterAttributesDict.Values)
            {
                var amount = attr.IncreaseBaseValue();
                levelUpdatas[i] = new AttributeModifiedData() { Type = attr.StatType.Type, IncreaseAmount = amount };
                 i++;
            }

            return levelUpdatas;
        }

        public float GetAttributeValue(StatTypes statType)
        {
            var characterAttribute = _characterAttributesDict[statType];
            return characterAttribute.GetAttributeValue();
        }
    }

    public struct AttributeModifiedData
    {
        public StatTypes Type;
        public float IncreaseAmount;
    }
}

