using RPGGame.SaveSystem;
using System;
using System.Collections;
using System.Collections.Generic;
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

        public void Save(Hero[] heroes)
        {
            var heroesWrapper = new HeroDataWrapper(heroes);
            _heroSaveSystem.Save(heroesWrapper);
        }

        public HeroDataWrapper Load()
        {
            _heroSaveSystem.Load(out HeroDataWrapper hero);
            return hero;
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

        public HeroDataWrapper(Hero[] heroes)
        {
            HeroDatas = new HeroData[heroes.Length];
            for (int i = 0; i < heroes.Length; i++)
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
            ID = hero._settings.ID;
            Level = hero._level;
            Experience = hero._experience;
        }
    }
}

