using RPGGame.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace RPGGame.Stats
{
    [CreateAssetMenu(menuName ="ScriptableObjects/Stats/BaseStatsContainer")]
    public class CharacterBaseStatsContainer : ScriptableObjectBase
    {
        public List<CharacterBaseStats> BaseStats;

        private Dictionary<string, CharacterBaseStats> _baseStatsDict;

        public override Task Initialize()
        {
            _baseStatsDict = new Dictionary<string, CharacterBaseStats>();
            for (int i = 0; i < BaseStats.Count; i++)
            {
                _baseStatsDict.Add(BaseStats[i].HeroID, BaseStats[i]);
            }

            return Task.CompletedTask;
        }

        public override Task Destroy()
        {
            _baseStatsDict.Clear(); 

            return Task.CompletedTask;
        }

        public bool TryGetBaseStatOfHero(string heroId, out CharacterBaseStats characterBaseStat)
        {
            if (_baseStatsDict.ContainsKey(heroId))
            {
                characterBaseStat = _baseStatsDict[heroId];
                return true;
            }

            characterBaseStat = null; 
            return false;
        }
    }
}
