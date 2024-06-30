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

        public Hero(HeroSettings heroSettings, HeroData heroData = null)
        {
            if(heroData == null)
            {
                Level = 1;
                Experience = 0;
            }
            else
            {
                Level = heroData.Level;
                Experience = heroData.Experience;
            }

         
            Settings = heroSettings;
            Stats = new CharacterStat(Settings.BaseStats, Level);
        }
    }
}
