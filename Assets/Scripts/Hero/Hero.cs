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
        public int HeroTeam;

        public Hero(HeroSettings heroSettings, HeroData heroData)
        {
            Level = heroData.Level;
            Experience = heroData.Experience;
            HeroTeam = heroData.HeroTeam;
            Settings = heroSettings;
            Stats = new CharacterStat(Settings.BaseStats, Level);
        }
    }
}
