using RPGGame.Config;
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

        public HeroData(string id, HeroTeam team, int startLevel = 1)
        {
            ID = id;
            HeroTeam = (int)team;
            if(startLevel > 1)
            {
                Level = startLevel;
                Experience = Level * GameConfig.Data.LevelIncreaseInterval;
            }
            else
            {
                Level = 1;
                Experience = 0;
            }
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

