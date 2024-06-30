using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGGame.SaveSystem;
using RPGGame.Player;
using System.Linq;

namespace RPGGame.Hero
{
    public class HeroCreator : MonoBehaviour
    {
        [SerializeField] private HeroSettingsContainer _heroSettings;

        private IHeroFactory _heroFactory = new HeroFactory();

        [ContextMenu("CreateHeroes")]
        public void CreateHeroes()
        {
            if (SaveSystemManager.Instance.HeroSaveSystem.HasSaveFile())
            {
                LoadHeroes();        
            }
            else
            {
                CreateNewHeroes();
            }
        }

        private void CreateNewHeroes()
        {
            for (int i = 0; i < 3; i++)
            {
                var heroSettings = _heroSettings.GetRandomHero();
                var hero = _heroFactory.Create(heroSettings);
                PlayerData.AddHero(hero);
            }
        }

        private void LoadHeroes()
        {
            var heroDatasWrapper = SaveSystemManager.Instance.HeroSaveSystem.Load();
            var heroeDatas = heroDatasWrapper.HeroDatas;
            var heroes = new Hero[heroeDatas.Length];
            for (int i = 0; i < heroeDatas.Length; i++)
            {
                if (_heroSettings.TryGetHeroSettings(heroeDatas[i].ID, out var settings))
                {
                    heroes[i] = _heroFactory.Create(settings, heroeDatas[i]);
                }
            }
            PlayerData.SetPlayerHeroes(heroes.ToList());
        }
    }
}

