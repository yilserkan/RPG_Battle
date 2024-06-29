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
        public int Level;
        public int Experience;
        public HeroSettings Settings;
        public CharacterStat Stats;

        public Hero(HeroSettings heroSettings)
        {
            Level = 1;
            Experience = 0;
            Settings = heroSettings;
            Stats = new CharacterStat(Settings.BaseStats, Level);
        }

        public Hero(HeroSettings heroSettings, HeroData heroData)
        {
            Level = heroData.Level;
            Experience = heroData.Experience;
            Settings = heroSettings;
            Stats = new CharacterStat(Settings.BaseStats, Level);
        }
    }
}
