using RPGGame.Game;
using RPGGame.Hero;
using RPGGame.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.SaveSystem
{
    public class SaveSystemManager : AbstractMonoSingleton<SaveSystemManager>
    {
        private HeroSaveSystem _heroSaveSystem;
        private GameSaveSystem _gameSaveSystem;

        public HeroSaveSystem HeroSaveSystem => _heroSaveSystem;
        public GameSaveSystem GameSaveSystem => _gameSaveSystem; 

        private void Start()
        {
            _heroSaveSystem = new HeroSaveSystem();
            _gameSaveSystem = new GameSaveSystem();
        }

        public void SaveAllSystems()
        {
            //_heroSaveSystem.Save();
            //_gameSaveSystem.Save();
        }

        public void LoadAllSystems()
        {
            //_heroSaveSystem.Load();
            //_gameSaveSystem.Load();
        }
    }
}