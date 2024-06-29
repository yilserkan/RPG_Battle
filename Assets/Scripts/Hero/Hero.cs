using RPGGame.Stats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Hero
{
    [Serializable]
    public class Hero
    {
        public int _level;
        public int _experience;
        public HeroSettings _settings;
        public CharacterStat _stats;

        public Hero(HeroSettings heroSettings)
        {
            _level = 1;
            _experience = 0;
            _settings = heroSettings;
            _stats = new CharacterStat(_settings.BaseStats, _level);
        }

        public Hero(HeroSettings heroSettings, HeroData heroData)
        {
            _level = heroData.Level;
            _experience = heroData.Experience;
            _settings = heroSettings;
            _stats = new CharacterStat(_settings.BaseStats, _level);
        }
    }
}
