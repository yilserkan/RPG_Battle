using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGGame.SaveSystem;
using RPGGame.Player;

namespace RPGGame.Hero
{
    public class HeroCreator : MonoBehaviour
    {
        [SerializeField] private HeroSettingsContainer _heroSettings;

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
            var heroSettings = _heroSettings.GetRandomHeroes();
            var heroes = new Hero[heroSettings.Length];
            for (int i = 0; i < heroSettings.Length; i++)
            {
                heroes[i] = new Hero(heroSettings[i]);
            }
            PlayerData.SetPlayerHeroes(heroes);
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
                    heroes[i] = new Hero(settings, heroeDatas[i]);
                }
            }
            PlayerData.SetPlayerHeroes(heroes);
        }
    }
}

