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

        public HeroSaveSystem HeroSaveSystem => _heroSaveSystem;

        private void Start()
        {
            _heroSaveSystem = new HeroSaveSystem();
        }

        public void SaveAllSystems()
        {
            _heroSaveSystem.Save();
        }
    }
}