using RPGGame.Player;
using RPGGame.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPGGame.Hero
{
    public class HeroSaveSystem
    {
        private JSONSaveSystem<HeroDataWrapper> _heroSaveSystem;
        private const string HERO_SAVE_SYSTEM_FILE_NAME = "HEROES";

        public HeroSaveSystem()
        {
            _heroSaveSystem = new JSONSaveSystem<HeroDataWrapper>(HERO_SAVE_SYSTEM_FILE_NAME);
        }

        public void Save()
        {
            var heroes = PlayerData.GetPlayerHeroes();
            var heroesWrapper = new HeroDataWrapper(heroes);
            _heroSaveSystem.Save(heroesWrapper);
        }

        public void Load()
        {
            if(HasSaveFile())
            {
                _heroSaveSystem.Load(out HeroDataWrapper heroDatasWrapper);
                PlayerData.SetPlayerHeroes(heroDatasWrapper);
            }
            else
            {
                PlayerData.CreateInitialHeroes();
            }
        }

        public bool HasSaveFile() 
        {
            return _heroSaveSystem.HasSaveFile();
        }
    }

    [Serializable]
    public class HeroDataWrapper
    {
        public HeroData[] HeroDatas;

        public HeroDataWrapper(List<Hero> heroes)
        {
            HeroDatas = new HeroData[heroes.Count];
            for (int i = 0; i < heroes.Count; i++)
            {
                HeroDatas[i] = new HeroData(heroes[i]);
            }
        }
    }

    [Serializable]
    public class HeroData
    {
        public string ID;
        public int Level;
        public int Experience;

        public HeroData(){}

        public HeroData(Hero hero)
        {
            ID = hero.Settings.ID;
            Level = hero.Level;
            Experience = hero.Experience;
        }
    }
}

