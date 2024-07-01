using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGGame.Hero;
using RPGGame.SaveSystem;
using RPGGame.Game;
using System.Linq;

namespace RPGGame.Player
{
    public static class PlayerData 
    {
        private static GameData _gameData;
        private static HeroSettingsContainer _heroSettingsContainer;
        private static HeroSettingsContainer _enemySettingsContainer;


        public static HeroSettingsContainer HeroSettingsContainer = _heroSettingsContainer;
        public static HeroSettingsContainer EnemySettingsContainer => _enemySettingsContainer;


        private static List<Hero.Hero> _playerHeroes = new List<Hero.Hero>();
        private static IHeroFactory _heroFactory = new HeroFactory();

        public static void Initialize(HeroSettingsContainer heroSettingsContainer, HeroSettingsContainer enemySettingsContainer)
        {
            _heroSettingsContainer = heroSettingsContainer;
            _enemySettingsContainer = enemySettingsContainer;   
        }

        public static void SetPlayerHeroes(HeroDataWrapper heroDatasWrapper)
        {
            var heroeDatas = heroDatasWrapper.HeroDatas;
            var heroes = new Hero.Hero[heroeDatas.Length];
            for (int i = 0; i < heroeDatas.Length; i++)
            {
              
                heroes[i] = _heroFactory.Create(_heroSettingsContainer, heroeDatas[i]);
            }

            _playerHeroes = heroes.ToList();
            SaveSystemManager.Instance.HeroSaveSystem.Save();
        }

        public static void AddHero(Hero.Hero hero)
        {
            _playerHeroes.Add(hero);
            SaveSystemManager.Instance.HeroSaveSystem.Save();
        }

        public static List<Hero.Hero> GetPlayerHeroes()
        {
            return _playerHeroes;
        }

        public static void CreateInitialHeroes()
        {
            for (int i = 0; i < 3; i++)
            {
                var hero = _heroFactory.CreateRandomHero(_heroSettingsContainer, HeroTeam.Player);
                AddHero(hero);
            }
        }

        public static void SetGameData(GameData gameData)
        {
            _gameData = gameData;
            SaveSystemManager.Instance.GameSaveSystem.Save();
        }

        public static GameData GetGameData()
        {
            return _gameData;
        }

        public static void CreateInitialGameData()
        {
            _gameData = new GameData();
            SaveSystemManager.Instance.GameSaveSystem.Save();
        }
    }
}
