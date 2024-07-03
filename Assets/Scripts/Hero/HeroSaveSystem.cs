using RPGGame.Game;
using RPGGame.Player;
using RPGGame.SaveSystem;
using RPGGame.Stats;
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

        public HeroData[] Load()
        {
            _heroSaveSystem.Load(out HeroDataWrapper heroDatasWrapper);
            PlayerData.SetPlayerHeroes(heroDatasWrapper);
            return heroDatasWrapper.HeroDatas;

            //if (HasSaveFile())
            //{
            //    _heroSaveSystem.Load(out HeroDataWrapper heroDatasWrapper);
            //    PlayerData.SetPlayerHeroes(heroDatasWrapper);
            //}
            //else
            //{
            //    PlayerData.CreateInitialHeroes();
            //}
        }

        public bool HasSaveFile() 
        {
            return _heroSaveSystem.HasSaveFile();
        }
    }
}

