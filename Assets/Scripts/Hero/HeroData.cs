using RPGGame.Game;
using System;

namespace RPGGame.Hero
{
    [Serializable]
    public class HeroData
    {
        public string ID;
        public int Level;
        public int Experience;
        public int HeroTeam;

        public HeroData(string id, HeroTeam team)
        {
            ID = id;
            Level = 1;
            Experience = 0;
            HeroTeam = (int)team;
        }

        public HeroData(Hero hero)
        {
            ID = hero.Settings.ID;
            Level = hero.Level;
            Experience = hero.Experience;
            HeroTeam = hero.HeroTeam;
        }

        public HeroData(HeroData heroData)
        {
            ID = heroData.ID;
            Level = heroData.Level;
            Experience = heroData.Experience;
            HeroTeam = heroData.HeroTeam;
        }
    }
}

