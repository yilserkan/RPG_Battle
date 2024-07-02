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

        public static HeroSettingsContainer HeroSettingsContainer = _heroSettingsContainer;

        private const int START_HERO_COUNT = 3;
        private const int RECEIVE_NEW_HERO_INTERVAL = 2;
        private const int LEVEL_INCREASE_INTERVAL = 3;
        private static Dictionary<string,Hero.Hero> _playerHeroes = new Dictionary<string, Hero.Hero>();
        private static IHeroFactory _heroFactory = new HeroFactory();

        public static void Initialize(HeroSettingsContainer heroSettingsContainer)
        {
            _heroSettingsContainer = heroSettingsContainer;
        }

        public static void SetPlayerHeroes(HeroDataWrapper heroDatasWrapper)
        {
            var heroeDatas = heroDatasWrapper.HeroDatas;
            var heroes = new Hero.Hero[heroeDatas.Length];
            for (int i = 0; i < heroeDatas.Length; i++)
            {
                var hero  = _heroFactory.Create(_heroSettingsContainer, heroeDatas[i]);
                _playerHeroes.Add(hero.Settings.ID, hero);
            }

            SaveSystemManager.Instance.HeroSaveSystem.Save();
        }

        public static void AddHero(Hero.Hero hero)
        {
            if (hero == null) return;

            _playerHeroes.Add(hero.Settings.ID, hero);
            SaveSystemManager.Instance.HeroSaveSystem.Save();
        }

        public static List<Hero.Hero> GetPlayerHeroes()
        {
            return _playerHeroes.Values.ToList();
        }

        public static void CreateInitialHeroes()
        {
            for (int i = 0; i < START_HERO_COUNT; i++)
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

        public static void UpdateGameData()
        {
            _gameData.PlayerMatchCount++;
            SaveSystemManager.Instance.GameSaveSystem.Save();

            bool receiveNewHero = _gameData.PlayerMatchCount % RECEIVE_NEW_HERO_INTERVAL == 0;
            if (receiveNewHero)
            {
                var hero = _heroFactory.CreateRandomHero(_heroSettingsContainer, HeroTeam.Player);
                AddHero(hero);
            }
        }

        public static void IncreaseHeroExperience(GameHero[] heroes)
        {
            for (int i = 0; i < heroes.Length; i++)
            {
                var heroId = heroes[i].Hero.Settings.ID;

                _playerHeroes[heroId].Experience++;
                if (_playerHeroes[heroId].Experience % LEVEL_INCREASE_INTERVAL == 0)
                {
                    _playerHeroes[heroId].Level++;
                    _playerHeroes[heroId].Stats.HandleOnPlayerLeveldUp();
                }
            }

            SaveSystemManager.Instance.HeroSaveSystem.Save();
        }

        public static GameData GetGameData()
        {
            return _gameData;
        }

        public static void SetActiveLevelData(LevelData levelData)
        {
            _gameData.ActiveLevelData = levelData;
            SaveSystemManager.Instance.GameSaveSystem.Save();
        }

        public static bool HasActiveLevelData()
        {
            return _gameData.ActiveLevelData != null;
        }

        public static void ResetActiveLevelData()
        {
            SetActiveLevelData(null);
        }

        public static void CreateInitialGameData()
        {
            _gameData = new GameData();
            SaveSystemManager.Instance.GameSaveSystem.Save();
        }
    }
}
